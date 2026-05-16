using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FoodHubCustomerUI.Models
{
    public class CustomerDTO
    {
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; } // <--- เติม ? ตรงนี้
    }
}
