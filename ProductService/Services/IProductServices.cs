using ProductService.Models;
using ProductService.DTOs;

namespace ProductService.Services
{
    public interface IProductServices
    {
        public Task<List<Product>> GetAllProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);
        public Task<Product> CreateProductAsync(ProductCreateDto productCreateDto);

    }
}
