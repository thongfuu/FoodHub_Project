using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FoodHubRestaurantUI.Models
{
    public class BookingDTO
    {
        [JsonPropertyName("bookingId")] public int BookingId { get; set; }
        [JsonPropertyName("customerId")] public int CustomerId { get; set; }
        [JsonPropertyName("customer")] public CustomerDTO? Customer { get; set; } // พ่วงชื่อลูกค้ามาด้วย
        [JsonPropertyName("bookingDate")] public DateTime BookingDate { get; set; }
        [JsonPropertyName("numPeople")] public int NumPeople { get; set; }
        [JsonPropertyName("status")] public string? Status { get; set; }
    }
}