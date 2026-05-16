using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodHubApi.Models
{
    public class Promotion
    {
        [Key]
        public int PromotionId { get; set; }

        [Required]
        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public float Discount { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
