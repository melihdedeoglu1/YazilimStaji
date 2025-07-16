using PaymentService.Models;
using PaymentService.DTOs;

namespace PaymentService.Services
{
    public interface IPaymentServices
    {
        
        Task<Payment> CreatePaymentAsync(CreatePaymentDto paymentDto);
        Task<Payment?> GetPaymentByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<bool> UpdatePaymentStatusAsync(Guid paymentId, bool isSuccess);
        Task<bool> RefundPaymentAsync(Guid orderId);
    }
}
