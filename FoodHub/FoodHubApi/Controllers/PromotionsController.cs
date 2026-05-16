using FoodHubApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // POST: api/Promotions (ร้านค้าสร้างโปรโมชั่น)
        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "สร้างโปรโมชั่นสำเร็จ", PromotionId = promotion.PromotionId });
        }

        // GET: api/Promotions/restaurant/1 (ดึงโปรโมชั่นทั้งหมดของร้านนั้น)
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetPromotionsByRestaurant(int restaurantId)
        {
            var promos = await _context.Promotions
                .Where(p => p.RestaurantId == restaurantId && p.IsActive == true)
                .ToListAsync();
            return Ok(promos);
        }
    }
}