using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using ProductService.DTOs;
using ProductService.Data;

namespace ProductService.Services
{
    public class ProductServices : IProductServices
    {
        private readonly ProductContext _context;
        public ProductServices(ProductContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {                      
           return await _context.Products.ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<Product> CreateProductAsync(ProductCreateDto productCreateDto)
        {
            var product = new Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                ImageUrl = productCreateDto.ImageUrl,
                StockQuantity = productCreateDto.StockQuantity,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
