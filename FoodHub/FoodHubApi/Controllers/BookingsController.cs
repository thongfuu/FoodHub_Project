using FoodHubApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetCustomerBookings(int customerId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Restaurant)
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return Ok(bookings);
        }

        // GET: api/Bookings/restaurant/1 (ดึงคิวให้ร้านดูที่ Dashboard)
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetRestaurantBookings(int restaurantId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Customer)
                .Where(b => b.RestaurantId == restaurantId)
                .OrderBy(b => b.BookingDate)
                .ToListAsync();
            return Ok(bookings);
        }

        // PUT: api/Bookings/5/status (ร้านกดยืนยัน/ปฏิเสธ หรือ เสร็จสิ้น)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] string newStatus)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound("ไม่พบคิวการจองนี้");

            booking.Status = newStatus;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "อัปเดตสถานะสำเร็จ", Status = booking.Status });
        }

        // POST: api/Bookings/cancel/5 (ลูกค้ายกเลิกจอง)
        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound("ไม่พบคิวการจองนี้");

            var customer = await _context.Customers.FindAsync(booking.CustomerId);
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

            await _context.SaveChangesAsync();
            return Ok(new { Message = "ยกเลิกการจองสำเร็จ", NewStatus = booking.Status });
        }
        // POST: api/Bookings (ลูกค้าจองโต๊ะ พร้อมระบบเช็คโต๊ะเต็ม และ เช็คเวลาเปิด-ปิดข้ามคืน)
        [HttpPost]
        public async Task<IActionResult> PostBooking([FromBody] Booking booking)
        {
            // 1. ดูว่าร้านนี้รับได้สูงสุดกี่คน และมีเวลาเปิดปิดตอนไหน
            var restaurant = await _context.Restaurants.FindAsync(booking.RestaurantId);
            if (restaurant == null) return NotFound("ไม่พบร้านอาหาร");

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
                            return BadRequest($"ขออภัย ร้านเปิดให้บริการช่วง {restaurant.OpenHours} น. เท่านั้น");
                        }
                    }
                }
                catch { /* ถ้ารูปแบบเวลาแปลกๆ ข้ามการเช็คไปเลย ป้องกัน API พัง */ }
            }

            // 2. คำนวณว่าใน slot เวลานั้นมีคนจอง (Confirmed และ Pending) ไปแล้วกี่คน
            var startTime = booking.BookingDate.AddMinutes(-59);
            var endTime = booking.BookingDate.AddMinutes(59);

            var currentBookedCount = await _context.Bookings
                .Where(b => b.RestaurantId == booking.RestaurantId &&
                            b.BookingDate >= startTime &&
                            b.BookingDate <= endTime &&
                            b.Status != "Cancelled" && b.Status != "Rejected")
                .SumAsync(b => b.NumPeople);

            // 3. ตรวจสอบว่าถ้าเพิ่มการจองใหม่นี้เข้าไป จะเกิน MaxCapacity ไหม
            if (currentBookedCount + booking.NumPeople > restaurant.MaxCapacity)
            {
                // ถ้าเกิน ให้ส่ง Error 400 กลับไปบอก WinForms
                return BadRequest($"ขออภัย ในช่วงเวลานี้โต๊ะเต็มแล้ว (เหลือที่นั่งอีกเพียง {restaurant.MaxCapacity - currentBookedCount} ที่)");
            }

            // 4. ผ่านทุกเงื่อนไข บันทึกการจอง
            booking.Status = "Pending";
            booking.CreatedAt = DateTime.UtcNow; // เก็บเวลาที่กดจอง

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // ส่งข้อความกลับไปบอก WinForms ว่าสำเร็จ
            return Ok(new { Message = "จองโต๊ะสำเร็จ รอร้านยืนยัน", BookingId = booking.BookingId });
        }
    }
}