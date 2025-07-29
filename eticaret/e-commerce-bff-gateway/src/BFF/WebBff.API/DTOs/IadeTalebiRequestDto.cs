namespace WebBff.API.DTOs
{
    public class IadeTalebiRequestDto
    {      
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public double RefundPrice { get; set; }
        public List<IadeEdilecekUrunDto> Products { get; set; } = new();
    }

    public class IadeEdilecekUrunDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty; 
        public int ProductQuantity { get; set; }
    }
}
