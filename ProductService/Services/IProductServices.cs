using ProductService.Models;
using ProductService.DTOs;

namespace ProductService.Services
{
    public interface IProductServices
    {
        public Task<List<Product>> GetAllProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);
        public Task<Product> CreateProductAsync(ProductCreateDto productCreateDto);

        public Task<Product> DecreaseStockAsync(int id, int quantity);
        //public Task<Product> IncreaseStockAsync(int id, int quantity);
    }
}
