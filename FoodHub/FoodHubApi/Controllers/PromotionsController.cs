using FoodHubApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FoodHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly FoodHubDbContext _context;

        public PromotionsController(FoodHubDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public Promotion CreatePromotion([FromBody] Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            _context.SaveChanges(); // ตัด Async ออก
            return promotion;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public List<Promotion> GetPromotionsByRestaurant(int restaurantId)
        {
            return _context.Promotions
                .Where(p => p.RestaurantId == restaurantId && p.IsActive == true)
                .ToList(); // ตัด Async ออก
        }
    }
}