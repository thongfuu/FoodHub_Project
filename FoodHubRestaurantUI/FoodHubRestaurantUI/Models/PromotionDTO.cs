using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FoodHubRestaurantUI.Models
{
    public class PromotionDTO
    {
        [JsonPropertyName("promotionId")] public int PromotionId { get; set; }
        [JsonPropertyName("restaurantId")] public int RestaurantId { get; set; }
        [JsonPropertyName("title")] public string? Title { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("discount")] public double Discount { get; set; }
        [JsonPropertyName("expireDate")] public DateTime ExpireDate { get; set; }
        [JsonPropertyName("isActive")] public bool IsActive { get; set; }
    }
}
