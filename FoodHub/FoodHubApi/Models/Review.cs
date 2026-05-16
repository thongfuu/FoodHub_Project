using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodHubApi.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [Required]
        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }

        // BookingId เป็น Nullable (optional) ตาม Data Dict 0..1
        public int? BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        [Required]
        [Range(1, 5)] // บังคับคะแนน 1-5 ดาว
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
