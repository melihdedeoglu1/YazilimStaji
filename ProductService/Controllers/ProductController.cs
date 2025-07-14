using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using ProductService.DTOs;
using ProductService.Services;


namespace ProductService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;

        public ProductController(IProductServices productService)
        {
            _productService = productService;
        }





        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            if (productCreateDto == null)
            {
                return BadRequest("Product data is null.");
            }
            var createdProduct = await _productService.CreateProductAsync(productCreateDto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);

        }



        [HttpPatch("{id}/decrease-stock")]
        public async Task<IActionResult> DecreaseStock(int id, [FromQuery] int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }
            var updatedProduct = await _productService.DecreaseStockAsync(id, quantity);
            if (updatedProduct == null)
            {
                return NotFound("Product not found or insufficient stock.");
            }
            return Ok(updatedProduct);
        }


    }
}
