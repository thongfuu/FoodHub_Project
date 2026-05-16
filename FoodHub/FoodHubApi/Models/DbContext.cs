using Microsoft.EntityFrameworkCore;

namespace FoodHubApi.Models
{
    public class FoodHubDbContext : DbContext
    {
        public FoodHubDbContext(DbContextOptions<FoodHubDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
    }
}
