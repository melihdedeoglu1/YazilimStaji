namespace WebBff.API.DTOs
{
    public class OrderDto
    {
        public DateTime? OrderDate { get; set; }

        public double TotalPrice { get; set; }

        public string Status { get; set; } 

        public ICollection<OrderItemDto>? OrderItems { get; set; }
    }
}
