using Urun.API.Models;

namespace Urun.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product?> UpdateAsync(int id,Product product);
        Task<bool> DeleteAsync(int id);
    }
}
