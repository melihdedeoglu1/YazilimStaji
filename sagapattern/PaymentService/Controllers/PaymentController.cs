using Microsoft.AspNetCore.Mvc;
using PaymentService.DTOs;
using PaymentService.Models;
using PaymentService.Services;
using System.Net.Http;


namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController :ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        private readonly HttpClient _httpClient;
        public PaymentController(IPaymentServices paymentServices, HttpClient httpClient)
        {
            _paymentServices = paymentServices;
            _httpClient = httpClient;
        }

        [HttpPost("payments")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto paymentDto)
        {
            
            if (paymentDto == null)
            {
                return BadRequest("Payment data is required.");
            }
            var payment = await _paymentServices.CreatePaymentAsync(paymentDto);
            return CreatedAtAction(nameof(GetPaymentByOrderId), new { orderId = payment.OrderId }, payment);
        }

        [HttpGet("payments/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrderId(Guid orderId)
        {
            var payment = await _paymentServices.GetPaymentByOrderIdAsync(orderId);
            if (payment == null)
            {
                return NotFound($"Payment with Order ID {orderId} not found.");
            }
            return Ok(payment);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentServices.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpPut("payments/{paymentId}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid paymentId, [FromBody] bool isSuccess)
        {
            var result = await _paymentServices.UpdatePaymentStatusAsync(paymentId, isSuccess);
            if (!result)
            {
                return NotFound($"Payment with ID {paymentId} not found.");
            }
            return NoContent();
        }

        [HttpPost("refund")]
        public async Task<IActionResult> Refund([FromBody] Guid orderId)
        {
            var result = await _paymentServices.RefundPaymentAsync(orderId);
            if (!result)
                return NotFound("Ödeme bulunamadı veya zaten iade edilmiş.");

            return Ok(new { message = "Ödeme iadesi başarılı." });
        }

    }
}
