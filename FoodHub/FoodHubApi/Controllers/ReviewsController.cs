using FoodHubApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly FoodHubDbContext _context;

        public ReviewsController(FoodHubDbContext context)
        {
            _context = context;
        }

        // POST: api/Reviews (ลูกค้าส่งรีวิว)
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] Review review)
        {
            review.CreatedAt = DateTime.UtcNow;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // อัปเดตคะแนนเฉลี่ยให้ร้านอาหาร (Logic คำนวณ AvgRating)
            var allReviews = await _context.Reviews.Where(r => r.RestaurantId == review.RestaurantId).ToListAsync();
            double avg = allReviews.Average(r => r.Rating);

            var restaurant = await _context.Restaurants.FindAsync(review.RestaurantId);
            if (restaurant != null)
            {
                restaurant.AvgRating = Math.Round(avg, 1);
                await _context.SaveChangesAsync();
            }

            return Ok(new { Message = "ขอบคุณสำหรับรีวิว!" });
        }

        // GET: api/Reviews/restaurant/1 (ดึงรีวิวไปโชว์ในหน้ารายละเอียดร้าน)
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetReviewsByRestaurant(int restaurantId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.Customer) // ดึงชื่อคนรีวิวมาด้วย
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return Ok(reviews);
        }
    }
}