using System.Text.Json.Serialization;

namespace OrderService.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; } 
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public Guid OrderId { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }
    }
}
