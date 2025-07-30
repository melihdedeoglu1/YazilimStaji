using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebBff.API.DTOs;

namespace WebBff.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaporlarController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger<RaporlarController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public RaporlarController(IHttpClientFactory httpClientFactory, ILogger<RaporlarController> logger, IPublishEndpoint publishEndpoint)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("en-cok-satin-alinan-urunler/{count}")]
        public async Task<IActionResult> GetBestSellingProducts(int count)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiGatewayClient");

                
                if (Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
                {
                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authHeaderValue);
                }

                
                var response = await client.GetAsync($"/rapor-api/en-cok-satin-alinan-urunler/{count}");

                if (response.IsSuccessStatusCode)
                {
                    var reportData = await response.Content.ReadFromJsonAsync<IEnumerable<RaporUrunPerformansDto>>();
                    return Ok(reportData);
                }

                _logger.LogWarning("Rapor servisinden veri çekilemedi. Status Code: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rapor servisine bağlanırken bir hata oluştu.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("en-cok-iade-edilen-urunler/{count}")]
        public async Task<IActionResult> GetMostRefundedProducts(int count) 
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiGatewayClient");


                if (Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
                {
                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authHeaderValue);
                }


                var response = await client.GetAsync($"/rapor-api/en-cok-iade-edilen-urunler/{count}");

                if (response.IsSuccessStatusCode)
                {
                    var reportData = await response.Content.ReadFromJsonAsync<IEnumerable<RaporUrunPerformansDto>>();
                    return Ok(reportData);
                }

                _logger.LogWarning("Rapor servisinden veri çekilemedi. Status Code: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rapor servisine bağlanırken bir hata oluştu.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("en-cok-siparis-veren-musteriler/{count}")]
        public async Task<IActionResult> GetTopCustomers(int count)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiGatewayClient");


                if (Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
                {
                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authHeaderValue);
                }


                var response = await client.GetAsync($"/rapor-api/en-cok-siparis-veren-musteriler/{count}");

                if (response.IsSuccessStatusCode)
                {
                    var reportData = await response.Content.ReadFromJsonAsync<IEnumerable<RaporMusteriSiparisiDto>>();
                    return Ok(reportData);
                }

                _logger.LogWarning("Rapor servisinden veri çekilemedi. Status Code: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rapor servisine bağlanırken bir hata oluştu.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("son-siparis-detaylari/{count}")]
        public async Task<IActionResult> GetRecentOrders(int count)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiGatewayClient");


                if (Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
                {
                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authHeaderValue);
                }


                var response = await client.GetAsync($"/rapor-api/son-siparis-detaylari/{count}");

                if (response.IsSuccessStatusCode)
                {
                    var reportData = await response.Content.ReadFromJsonAsync<IEnumerable<RaporSiparisDetay>>();
                    return Ok(reportData);
                }

                _logger.LogWarning("Rapor servisinden veri çekilemedi. Status Code: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rapor servisine bağlanırken bir hata oluştu.");
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
