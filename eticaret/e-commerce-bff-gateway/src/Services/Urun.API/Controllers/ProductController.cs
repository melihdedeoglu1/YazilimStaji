using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Urun.API.DTOs;
using Urun.API.Services;

namespace Urun.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        private readonly ILogger<ProductController> _logger;

        private readonly IValidator<ProductForCreateDto> _createValidator;
        private readonly IValidator<ProductForUpdateDto> _updateValidator;

        public ProductController(IProductService productService, IValidator<ProductForCreateDto> createValidator, IValidator<ProductForUpdateDto> updateValidator, ILogger<ProductController> logger) 
        {
            _productService = productService;
            _createValidator = createValidator; 
            _updateValidator = updateValidator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Ürün detayı istendi. ProductId: {ProductId}", id);

            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Ürün bulunamadı. ProductId: {ProductId}", id);
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreateDto productDto)
        {
            
            var validationResult = await _createValidator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                
                return BadRequest(validationResult.Errors);
            }
            

            var createdProduct = await _productService.CreateAsync(productDto);

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductForUpdateDto productDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {

                return BadRequest(validationResult.Errors);
            }

            var updatedProduct = await _productService.UpdateAsync(id, productDto);

            if (updatedProduct == null)
            {
                return NotFound(); 
            }

            return NoContent(); 
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _productService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound(); 
            }

            return NoContent(); 
        }



    }
}
