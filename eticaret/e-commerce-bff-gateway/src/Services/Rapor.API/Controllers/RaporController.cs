using Microsoft.AspNetCore.Mvc;
using Rapor.API.Services;
using System.Threading.Tasks;

namespace Rapor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaporlarController : ControllerBase
    {
        private readonly IRaporService _raporService; 
        
        public RaporlarController(IRaporService raporService)
        {
            _raporService = raporService;
        }

        [HttpGet("en-cok-satin-alinan-urunler/{count}")]
        public async Task<IActionResult> GetBestSellingProducts(int count )
        {
            var result = await _raporService.GetBestSellingProductsAsync(count);
            return Ok(result);
        }

        [HttpGet("en-cok-iade-edilen-urunler/{count}")]
        public async Task<IActionResult> GetMostRefundedProducts(int count)
        {
            var result = await _raporService.GetMostRefundedProductsAsync(count);
            return Ok(result);
        }

        [HttpGet("en-cok-siparis-veren-musteriler/{count}")]
        public async Task<IActionResult> GetTopCustomers(int count)
        {
            var result = await _raporService.GetTopCustomersAsync(count);
            return Ok(result);
        }

        [HttpGet("son-siparis-detaylari/{count}")]
        public async Task<IActionResult> GetRecentOrders(int count)
        {
            var result = await _raporService.GetRecentOrdersAsync(count);
            return Ok(result);
        }
    }
}
