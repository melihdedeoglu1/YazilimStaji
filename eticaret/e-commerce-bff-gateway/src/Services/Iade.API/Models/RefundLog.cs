namespace Iade.API.Models
{
    public class RefundLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        
        public double RefundPrice { get; set; }

        public List<RefundProduct> Products { get; set; }
    }
}
