using System.Text.Json.Serialization;

namespace SagaOrchestratorService.DTOs
{
    public class CreateOrderDto
    {

        [JsonPropertyName("customerId")]
        public Guid CustomerId { get; set; }
        public string AddressId { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItemDto> Items { get; set; }

    }
}
