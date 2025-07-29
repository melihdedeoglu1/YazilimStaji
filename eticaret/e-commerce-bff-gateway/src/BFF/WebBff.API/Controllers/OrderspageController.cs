using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebBff.API.DTOs;
using Shared.Contracts;

namespace WebBff.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderspageController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger<OrderspageController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderspageController(IHttpClientFactory httpClientFactory, ILogger<OrderspageController> logger, IPublishEndpoint publishEndpoint)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }


        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayClient");

            var orderspageData = new OrderspageDto {
            
                FeaturedOrders = Enumerable.Empty<OrderDto>()
            };


            if (Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
            {
                try
                {

                    var userRequest = new HttpRequestMessage(HttpMethod.Get, "/user-api/me");
                    userRequest.Headers.Authorization = AuthenticationHeaderValue.Parse(authHeaderValue);

                    var userResponse = await client.SendAsync(userRequest);

                    if (userResponse.IsSuccessStatusCode)
                    {
                        orderspageData.CurrentUser = await userResponse.Content.ReadFromJsonAsync<UserDto>();
                    }
                    else
                    {
                        _logger.LogWarning("Kullanici.API servisinden kullanıcı bilgisi çekilemedi. Status Code: {StatusCode}", userResponse.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Kullanici.API servisinden kullanıcı bilgisi çekilirken bir hata oluştu.");
                }


                try
                {
                    var orderRequest = new HttpRequestMessage(HttpMethod.Get, "/order-api/Order");
                    orderRequest.Headers.Authorization = AuthenticationHeaderValue.Parse(authHeaderValue);

                    var orderResponse = await client.SendAsync(orderRequest);

                    if (orderResponse.IsSuccessStatusCode) 
                    {
                        orderspageData.FeaturedOrders = await orderResponse.Content.ReadFromJsonAsync<IEnumerable<OrderDto>>();
                    }
                    else
                    {
                        orderspageData.FeaturedOrders = Enumerable.Empty<OrderDto>();

                        _logger.LogWarning("Siparis.API servisinden kullanıcı sipariş bilgileri çekilemedi. Status Code: {StatusCode}", orderResponse.StatusCode);
                    }

                }
                catch (Exception ex)
                {
                    orderspageData.FeaturedOrders = Enumerable.Empty<OrderDto> ();
                    _logger.LogError(ex, "Siparis.API servisinden kullanıcı siparişleri bilgisi çekilirken bir hata oluştu.");
                }



            }


            return Ok(orderspageData);


        }



        [HttpPost("iade-talebi")]
        public async Task<IActionResult> IadeTalebiOlustur([FromBody] IadeTalebiRequestDto request)
        {
            
            var iadeTalebiEvent = new SiparisIadeTalebiEvent
            {                
                OrderId = request.OrderId,
                UserId = request.UserId, 
                RefundPrice = request.RefundPrice,
                Products = request.Products.Select(p => new IptalUrun
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductQuantity = p.ProductQuantity
                }).ToList()
            };

            
            await _publishEndpoint.Publish(iadeTalebiEvent);

            _logger.LogInformation("{OrderId} numaralı sipariş için iade talebi alındı ve yayınlandı.", request.OrderId);

            
            return Ok(new { Message = "İade talebiniz başarıyla alınmıştır. Durumunu sipariş detay sayfasından takip edebilirsiniz." });
        }

    }
}
