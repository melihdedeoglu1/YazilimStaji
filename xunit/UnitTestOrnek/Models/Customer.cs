namespace UnitTestOrnek.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;      

        public ICollection<Order>? Orders { get; set; }

    }
}
