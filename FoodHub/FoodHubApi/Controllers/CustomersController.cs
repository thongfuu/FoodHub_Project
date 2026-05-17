using FoodHubApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FoodHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly FoodHubDbContext _context;

        public CustomersController(FoodHubDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Customer> GetCustomers()
        {
            return _context.Customers.ToList(); // ตัด Async ออก
        }
    }
}