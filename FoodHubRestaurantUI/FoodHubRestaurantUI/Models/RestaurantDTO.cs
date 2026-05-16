using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FoodHubRestaurantUI.Models
{
    public class RestaurantDTO
    {
        [JsonPropertyName("restaurantId")] public int RestaurantId { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("maxCapacity")] public int MaxCapacity { get; set; } = 20;
    }
}