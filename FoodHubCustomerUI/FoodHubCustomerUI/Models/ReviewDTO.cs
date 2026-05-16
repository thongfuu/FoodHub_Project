using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FoodHubCustomerUI.Models
{
    public class ReviewDTO
    {
        [JsonPropertyName("reviewId")] public int ReviewId { get; set; }

        // ข้อมูลลูกค้าที่พ่วงมาด้วย
        [JsonPropertyName("customer")] public CustomerDTO? Customer { get; set; }

        [JsonPropertyName("rating")] public int Rating { get; set; }
        [JsonPropertyName("comment")] public string? Comment { get; set; }
        [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
    }
}