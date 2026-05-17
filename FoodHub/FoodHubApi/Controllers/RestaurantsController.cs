using FoodHubApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly FoodHubDbContext _context;

        public RestaurantsController(FoodHubDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Restaurant> GetAllRestaurants()
        {
            return _context.Restaurants.ToList(); // ตัด Async ออก
        }

        [HttpGet("{id}")]
        public Restaurant GetRestaurantById(int id)
        {
            var restaurant = _context.Restaurants.Find(id); // ตัด Async ออก
            if (restaurant == null) throw new Exception("ไม่พบร้านอาหารนี้"); // ใช้ throw แทน NotFound

            return restaurant;
        }
    }
}