namespace ShippingService.DTOs
{
    public class ShippingCreateRequestDto
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public string? AddressId { get; set; }
    }
}
