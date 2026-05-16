using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using FoodHubCustomerUI.Models;

namespace FoodHubCustomerUI.Models
{
    public class BookingDTO
    {
        [JsonPropertyName("bookingId")] public int BookingId { get; set; }
        [JsonPropertyName("restaurantId")] public int RestaurantId { get; set; }
        [JsonPropertyName("bookingDate")] public DateTime BookingDate { get; set; }
        [JsonPropertyName("numPeople")] public int NumPeople { get; set; }
        [JsonPropertyName("status")] public string? Status { get; set; }

        // ข้อมูลร้านอาหารที่แถมมาด้วยตอนดึงประวัติ
        [JsonPropertyName("restaurant")] public RestaurantDTO? Restaurant { get; set; }
    }
}
