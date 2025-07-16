namespace ShippingService.Models
{
    public enum ShippingStatus
    {
        Hazirlaniyor = 0,
        KargoyaVerildi = 1,
        TeslimEdildi = 2,
        IptalEdildi = 3
    }

    public class Shipping
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public string? AddressId { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public ShippingStatus Status { get; set; } = ShippingStatus.Hazirlaniyor;
    }
}
