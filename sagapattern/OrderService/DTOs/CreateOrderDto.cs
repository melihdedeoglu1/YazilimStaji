using System.Text.Json.Serialization;

namespace OrderService.DTOs
{
    public class CreateOrderDto
    {
        [JsonPropertyName("customerId")]
        public Guid CustomerId { get; set; }

        [JsonPropertyName("items")]
        public List<CreateOrderItemDto> Items { get; set; }
    }
}
