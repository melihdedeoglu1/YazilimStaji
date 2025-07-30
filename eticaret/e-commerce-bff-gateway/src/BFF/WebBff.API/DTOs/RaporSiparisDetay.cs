namespace WebBff.API.DTOs
{
    public class RaporSiparisDetay
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public string OrderStatus { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;
        public double ProductPrice { get; set; }

        public int ProductQuantity { get; set; }
    }
}
