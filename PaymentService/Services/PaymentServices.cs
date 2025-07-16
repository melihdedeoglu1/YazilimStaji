using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Models;
using PaymentService.Services;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PaymentService.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly PaymentContext _context;
        private readonly HttpClient _client;
        public PaymentServices(PaymentContext context, HttpClient client)
        {
            _context = context;
            _client = client;
        }

        public async Task<Payment> CreatePaymentAsync(CreatePaymentDto paymentDto)
        {

            var request = new HttpRequestMessage(HttpMethod.Patch,
    $"https://localhost:7068/api/customer/{paymentDto.CustomerId}/decrease-balance?amount={paymentDto.Amount}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Bakiye düşülemedi: " + error);
            }

            // Ödeme kaydı oluştur
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                CustomerId = paymentDto.CustomerId,
                OrderId = paymentDto.OrderId,
                Amount = paymentDto.Amount,
                IsSuccess = true, // ödeme başarılı olduğu için true
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }


        public async Task<Payment?> GetPaymentByOrderIdAsync(Guid orderId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid paymentId, bool isSuccess)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null) return false;
            payment.IsSuccess = isSuccess;
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RefundPaymentAsync(Guid orderId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);

            if (payment == null || payment.IsSuccess != true)
                return false;

            // Customer bakiyesini geri artır (Age = Balance)
            var request = new HttpRequestMessage(HttpMethod.Patch,
                $"https://localhost:7068/api/customer/{payment.CustomerId}/increase-balance?amount={payment.Amount}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return false;

            // Payment kaydını başarısız olarak işaretle
            payment.IsSuccess = false;
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
