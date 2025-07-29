namespace Iade.API.Models
{
    public class RefundProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int ProductQuantity { get; set; }


    }
}
