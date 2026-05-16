using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FoodHubCustomerUI.Models
{
    public class RestaurantDTO
    {
        [JsonPropertyName("restaurantId")]
        public int RestaurantId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("avgRating")]
        public double? AvgRating { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("openHours")]
        public string? OpenHours { get; set; }
        [JsonPropertyName("maxCapacity")]
        public int MaxCapacity { get; set; } = 20;
    }
}