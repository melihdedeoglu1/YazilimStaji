namespace OrderService.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreatedAt { get; set; } 
        public ICollection<OrderItem> Items { get; set; }
    }
}
