namespace CustomerService.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int? Age { get; set; }
        public string? Name { get; set; }

    }
}
