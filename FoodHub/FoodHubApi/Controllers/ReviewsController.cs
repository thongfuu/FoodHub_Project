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
    public class ReviewsController : ControllerBase
    {
        private readonly FoodHubDbContext _context;

        public ReviewsController(FoodHubDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public Review CreateReview([FromBody] Review review)
        {
            review.CreatedAt = DateTime.UtcNow;
            _context.Reviews.Add(review);
            _context.SaveChanges(); // เซฟครั้งที่ 1

            // อัปเดตคะแนนเฉลี่ยให้ร้านอาหาร
            var allReviews = _context.Reviews.Where(r => r.RestaurantId == review.RestaurantId).ToList();
            if (allReviews.Any())
            {
                double avg = allReviews.Average(r => r.Rating);
                var restaurant = _context.Restaurants.Find(review.RestaurantId);

                if (restaurant != null)
                {
                    restaurant.AvgRating = Math.Round(avg, 1);
                    _context.SaveChanges(); // เซฟครั้งที่ 2
                }
            }

            return review;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public List<Review> GetReviewsByRestaurant(int restaurantId)
        {
            return _context.Reviews
                .Include(r => r.Customer)
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList(); // ตัด Async ออก
        }
    }
}