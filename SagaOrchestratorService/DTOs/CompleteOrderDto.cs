namespace SagaOrchestratorService.DTOs
{
    public class CompleteOrderDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string AddressId { get; set; }
        public double Amount { get; set; }
    }
}
