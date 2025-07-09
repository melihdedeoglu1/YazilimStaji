using Microsoft.AspNetCore.Mvc;
using Staj_ETicaretornek.DTOs;
using Staj_ETicaretornek.Services;

namespace Staj_ETicaretornek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService) {

            _productService = productService;
        
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductInfo productInfo)
        {
            if (productInfo == null)
            {
                return BadRequest("Product information cannot be null.");
            }
            try
            {
                var product = await _productService.CreateAsync(productInfo);
                return CreatedAtAction(nameof(CreateProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
