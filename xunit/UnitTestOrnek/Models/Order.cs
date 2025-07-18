namespace UnitTestOrnek.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public double Total { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; } 
    }
}
