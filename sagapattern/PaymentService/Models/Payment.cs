namespace PaymentService.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public double Amount { get; set; }
        public bool? IsSuccess { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
