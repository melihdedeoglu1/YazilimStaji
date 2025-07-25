using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebBff.API.DTOs;

namespace WebBff.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomepageController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomepageController> _logger;

        public HomepageController(IHttpClientFactory httpClientFactory, ILogger<HomepageController> logger)
        { 
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayClient");

            
            var homepageData = new HomepageDto
            {
                
                FeaturedProducts = Enumerable.Empty<ProductDto>()
            };


            try 
            {
                var productsResponse = await client.GetAsync("/product-api/Product");

                if (productsResponse.IsSuccessStatusCode)
                {
                    homepageData.FeaturedProducts = await productsResponse.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                }
                else
                {
                    homepageData.FeaturedProducts = Enumerable.Empty<ProductDto>();

                    _logger.LogWarning("Urun.API servisinden ürünler çekilemedi. Status Code: {StatusCode}", productsResponse.StatusCode);
                }

            } 
            catch (Exception ex) 
            {
                homepageData.FeaturedProducts = Enumerable.Empty<ProductDto>();

                _logger.LogError(ex, "Urun.API servisine bağlanırken bir hata oluştu.");
            }


            if (Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
            {
                try
                {
                    
                    var userRequest = new HttpRequestMessage(HttpMethod.Get, "/user-api/me");
                    userRequest.Headers.Authorization = AuthenticationHeaderValue.Parse(authHeaderValue);

                    var userResponse = await client.SendAsync(userRequest);

                    if (userResponse.IsSuccessStatusCode)
                    {
                        homepageData.CurrentUser = await userResponse.Content.ReadFromJsonAsync<UserDto>();
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
            }

            return Ok(homepageData);

        }



           
    }
}
