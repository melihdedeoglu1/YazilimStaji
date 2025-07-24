namespace Siparis.API.DTOs
{
    public class OrderForCreateDto
    {
        public List<OrderItemForCreateDto> OrderItems { get; set; } = new List<OrderItemForCreateDto>();
    }
}
