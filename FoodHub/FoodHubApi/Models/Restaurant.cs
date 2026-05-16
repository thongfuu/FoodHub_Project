using System.ComponentModel.DataAnnotations;

namespace FoodHubApi.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }

        [Required, MaxLength(130)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string OwnerName { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }
        public string? Address { get; set; }
        [MaxLength(50)]
        public string? Category { get; set; }
        [MaxLength(100)]
        public string? OpenHours { get; set; }
        public double? AvgRating { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int MaxCapacity { get; set; } = 20;
    }
}
