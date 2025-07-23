using Urun.API.Models;
using Urun.API.DTOs;

namespace Urun.API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(ProductForCreateDto productDto);
        Task<Product?> UpdateAsync(int id, ProductForUpdateDto productDto);
        Task<bool> DeleteAsync(int id);

    }
}
