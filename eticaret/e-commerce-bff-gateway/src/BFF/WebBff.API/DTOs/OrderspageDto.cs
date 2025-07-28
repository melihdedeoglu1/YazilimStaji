namespace WebBff.API.DTOs
{
    public class OrderspageDto
    {
        public UserDto? CurrentUser { get; set; }

        public IEnumerable<OrderDto> FeaturedOrders { get; set; }
    }
}
