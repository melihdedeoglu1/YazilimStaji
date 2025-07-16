using Microsoft.AspNetCore.Mvc;
using SagaOrchestratorService.DTOs;
using System.Net.Http.Headers;

namespace SagaOrchestratorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderSagaController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public OrderSagaController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string orderService = "https://localhost:7096";
        private readonly string paymentService = "https://localhost:7281";
        private readonly string shippingService = "https://localhost:7061";
        private readonly string productService = "https://localhost:7156";

        [HttpPost("start-order")] 
        public async Task<IActionResult> StartOrder([FromBody] CreateOrderDto dto)
        {
            var token = GetToken();
            if (token == null) return Unauthorized("Bearer token gerekli");

            try
            {
                
                var orderResp = await AuthorizedPost($"{orderService}/api/order/create", dto, token);
                if (!orderResp.IsSuccessStatusCode)
                    return BadRequest("Sipariş oluşturulamadı.");

                var data = await orderResp.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
                var orderId = data["orderId"];

                
                foreach (var item in dto.Items)
                {
                    var stockResp = await AuthorizedPatch(
                        $"{productService}/api/product/{item.ProductId}/decrease-stock?quantity={item.Quantity}",
                        token);

                    if (!stockResp.IsSuccessStatusCode)
                    {
                        await CancelOrder(orderId, token);
                        return BadRequest("Stok yetersiz. Sipariş iptal edildi.");
                    }
                }

                return Ok(new { orderId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"StartOrder hatası: {ex.Message}");
            }
        }

        [HttpPost("complete-order")] 
        public async Task<IActionResult> CompleteOrder([FromBody] CompleteOrderDto dto)
        {
            var token = GetToken();
            if (token == null) return Unauthorized("Bearer token gerekli");

            try
            {
                
                var paymentDto = new { orderId = dto.OrderId, customerId = dto.CustomerId, amount = dto.Amount };
                var payResp = await AuthorizedPost($"{paymentService}/api/payment/payments", paymentDto, token);
                if (!payResp.IsSuccessStatusCode)
                {
                    await RollbackStock(dto.OrderId, token);
                    await CancelOrder(dto.OrderId, token);
                    return BadRequest("Ödeme başarısız. Sipariş iptal edildi.");
                }

                // 2. Kargo oluştur
                var shippingDto = new { customerId = dto.CustomerId, orderId = dto.OrderId, addressId = dto.AddressId };
                var shipResp = await AuthorizedPost($"{shippingService}/api/shipping", shippingDto, token);
                if (!shipResp.IsSuccessStatusCode)
                {
                    await _httpClient.PostAsJsonAsync($"{paymentService}/api/payment/refund", dto.OrderId);
                    await RollbackStock(dto.OrderId, token);
                    await CancelOrder(dto.OrderId, token);
                    return BadRequest("Kargo başarısız. İşlemler geri alındı.");
                }

                return Ok(new { message = "Sipariş başarıyla tamamlandı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"CompleteOrder hatası: {ex.Message}");
            }
        }

        
        private string? GetToken()
        {
            return Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
        }

        private async Task CancelOrder(Guid orderId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{orderService}/api/order/cancel/{orderId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            await _httpClient.SendAsync(request);
        }

        private async Task RollbackStock(Guid orderId, string token)
        {
            var orderDetailsResp = await AuthorizedGet($"{orderService}/api/order/details/{orderId}", token);
            if (!orderDetailsResp.IsSuccessStatusCode) return;

            var orderDetails = await orderDetailsResp.Content.ReadFromJsonAsync<OrderDetailsDto>();
            if (orderDetails == null || orderDetails.Items == null) return;

            foreach (var item in orderDetails.Items)
            {
                var rollbackUrl = $"{productService}/api/product/{item.ProductId}/increase-stock?quantity={item.Quantity}";
                var request = new HttpRequestMessage(HttpMethod.Patch, rollbackUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                await _httpClient.SendAsync(request);
            }
        }

        private async Task<HttpResponseMessage> AuthorizedGet(string url, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.SendAsync(request);
        }

        private async Task<HttpResponseMessage> AuthorizedPost(string url, object body, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(body)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.SendAsync(request);
        }

        private async Task<HttpResponseMessage> AuthorizedPatch(string url, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.SendAsync(request);
        }
    }

   
}