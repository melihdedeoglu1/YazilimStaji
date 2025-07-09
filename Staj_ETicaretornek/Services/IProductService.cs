using Staj_ETicaretornek.Models;
using Staj_ETicaretornek.DTOs;


namespace Staj_ETicaretornek.Services
{
    public interface IProductService
    {
        Task<Product> CreateAsync(ProductInfo productInfo);

        Task<List<Product>> GetAllAsync();

        //Task<bool> DeleteAsync(int id);

        //Task<Product> UpdateAsync(int id, Product updatedProduct);

    }
}
