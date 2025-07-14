namespace PaymentService.DTOs
{
    public class CreatePaymentDto
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public double Amount { get; set; }
    }
}
