using FoodHubApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly FoodHubDbContext _context;

        public BookingsController(FoodHubDbContext context)
        {
            _context = context;
        }

        // GET: api/Bookings/customer/1 (ดึงประวัติการจองของลูกค้า)
        [HttpGet("customer/{customerId}")]
        public List<Booking> GetCustomerBookings(int customerId)
        {
            var bookings = _context.Bookings
                .Include(b => b.Restaurant)
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.BookingDate)
                .ToList(); // ตัด Async ออก

            return bookings; // คืนค่า List ตรงๆ ไม่ต้องใส่ Ok()
        }

        // GET: api/Bookings/restaurant/1 (ดึงคิวให้ร้านดูที่ Dashboard)
        [HttpGet("restaurant/{restaurantId}")]
        public List<Booking> GetRestaurantBookings(int restaurantId)
        {
            var bookings = _context.Bookings
                .Include(b => b.Customer)
                .Where(b => b.RestaurantId == restaurantId)
                .OrderBy(b => b.BookingDate)
                .ToList(); // ตัด Async ออก

            return bookings;
        }

        // PUT: api/Bookings/5/status (ร้านกดยืนยัน/ปฏิเสธ หรือ เสร็จสิ้น)
        [HttpPut("{id}/status")]
        public Booking UpdateBookingStatus(int id, [FromBody] string newStatus)
        {
            var booking = _context.Bookings.Find(id); // ตัด Async ออก
            if (booking == null) throw new Exception("ไม่พบคิวการจองนี้"); // ใช้ throw Exception แทน NotFound

            booking.Status = newStatus;
            _context.SaveChanges(); // ตัด Async ออก

            return booking;
        }

        // POST: api/Bookings/cancel/5 (ลูกค้ายกเลิกจอง)
        [HttpPost("cancel/{id}")]
        public Booking CancelBooking(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null) throw new Exception("ไม่พบคิวการจองนี้");

            var customer = _context.Customers.Find(booking.CustomerId);
            TimeSpan timeUntilBooking = booking.BookingDate - DateTime.UtcNow;

            if (timeUntilBooking <= TimeSpan.FromHours(2))
            {
                booking.Status = "LateCancelled";
                if (customer != null) customer.LateCancelCount += 1;
            }
            else
            {
                booking.Status = "Cancelled";
            }

            _context.SaveChanges();

            return booking;
        }

        // POST: api/Bookings (ลูกค้าจองโต๊ะ พร้อมระบบเช็คโต๊ะเต็ม และ เช็คเวลาเปิด-ปิดข้ามคืน)
        [HttpPost]
        public Booking PostBooking([FromBody] Booking booking)
        {
            // 1. ดูว่าร้านนี้รับได้สูงสุดกี่คน และมีเวลาเปิดปิดตอนไหน
            var restaurant = _context.Restaurants.Find(booking.RestaurantId);
            if (restaurant == null) throw new Exception("ไม่พบร้านอาหาร"); // โยน Exception แทน

            if (!string.IsNullOrEmpty(restaurant.OpenHours) && restaurant.OpenHours.Contains("-"))
            {
                try
                {
                    var hours = restaurant.OpenHours.Split('-');
                    if (TimeSpan.TryParse(hours[0].Trim(), out TimeSpan openTime) &&
                        TimeSpan.TryParse(hours[1].Trim(), out TimeSpan closeTime))
                    {
                        var bookingTime = booking.BookingDate.TimeOfDay;
                        bool isClosed = false;

                        if (openTime < closeTime)
                        {
                            if (bookingTime < openTime || bookingTime > closeTime)
                            {
                                isClosed = true;
                            }
                        }
                        else
                        {
                            if (bookingTime > closeTime && bookingTime < openTime)
                            {
                                isClosed = true;
                            }
                        }

                        if (isClosed)
                        {
                            // ใช้ throw Exception แทน BadRequest
                            throw new Exception($"ขออภัย ร้านเปิดให้บริการช่วง {restaurant.OpenHours} น. เท่านั้น");
                        }
                    }
                }
                catch (Exception ex) when (ex.Message.Contains("ขออภัย"))
                {
                    throw; // ปล่อย Exception ที่เราตั้งใจดักไว้ให้เด้งออกไป
                }
                catch { /* ถ้ารูปแบบเวลาแปลกๆ ข้ามการเช็คไปเลย ป้องกัน API พัง */ }
            }

            // 2. คำนวณว่าใน slot เวลานั้นมีคนจอง (Confirmed และ Pending) ไปแล้วกี่คน
            var startTime = booking.BookingDate.AddMinutes(-59);
            var endTime = booking.BookingDate.AddMinutes(59);

            var currentBookedCount = _context.Bookings
                .Where(b => b.RestaurantId == booking.RestaurantId &&
                            b.BookingDate >= startTime &&
                            b.BookingDate <= endTime &&
                            b.Status != "Cancelled" && b.Status != "Rejected")
                .Sum(b => b.NumPeople); // เปลี่ยนจาก SumAsync เป็น Sum ธรรมดา

            // 3. ตรวจสอบว่าถ้าเพิ่มการจองใหม่นี้เข้าไป จะเกิน MaxCapacity ไหม
            if (currentBookedCount + booking.NumPeople > restaurant.MaxCapacity)
            {
                // ใช้ throw Exception แทน BadRequest
                throw new Exception($"ขออภัย ในช่วงเวลานี้โต๊ะเต็มแล้ว (เหลือที่นั่งอีกเพียง {restaurant.MaxCapacity - currentBookedCount} ที่)");
            }

            // 4. ผ่านทุกเงื่อนไข บันทึกการจอง
            booking.Status = "Pending";
            booking.CreatedAt = DateTime.UtcNow; // เก็บเวลาที่กดจอง

            _context.Bookings.Add(booking);
            _context.SaveChanges(); // เปลี่ยนเป็นเซฟแบบธรรมดา

            // คืนค่า Object ตรงๆ
            return booking;
        }
    }
}