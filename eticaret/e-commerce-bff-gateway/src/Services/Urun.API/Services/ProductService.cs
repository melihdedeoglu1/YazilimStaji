using Urun.API.Repositories;
using Urun.API.Models;
using Urun.API.DTOs;

namespace Urun.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> CreateAsync(ProductForCreateDto productDto)
        {
            
            var newProduct = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                CreatedAt = DateTime.UtcNow
            };

            return await _productRepository.CreateAsync(newProduct);
        }

        public async Task<Product?> UpdateAsync(int id, ProductForUpdateDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity
            };

            return await _productRepository.UpdateAsync(id, product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

    }
}
