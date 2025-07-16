namespace OrderService.DTOs
{
    public class OrderDetailsDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
    }
}

