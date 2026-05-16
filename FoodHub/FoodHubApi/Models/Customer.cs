using System.ComponentModel.DataAnnotations;

namespace FoodHubApi.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ฟิลด์เก็บบทลงโทษเวลายกเลิกช้า
        public int LateCancelCount { get; set; } = 0;
    }
}
