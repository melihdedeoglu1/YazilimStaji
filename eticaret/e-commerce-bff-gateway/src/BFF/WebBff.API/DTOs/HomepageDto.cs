namespace WebBff.API.DTOs
{
    public class HomepageDto
    {
        public UserDto? CurrentUser { get; set; }
        public IEnumerable<ProductDto> FeaturedProducts { get; set; }
    }
}
