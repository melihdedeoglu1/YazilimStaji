using Microsoft.EntityFrameworkCore;
using Staj_ETicaretornek.Data;
using Staj_ETicaretornek.Models;
using Staj_ETicaretornek.DTOs;

namespace Staj_ETicaretornek.Services
{
    public class ProductService : IProductService
    {
        private readonly Context _context;
        public ProductService(Context context)
        {
            _context = context;
        }
        public async Task<Product> CreateAsync(ProductInfo productInfo)
        {
            if (productInfo == null)
            {
                throw new ArgumentNullException(nameof(productInfo), "Product information cannot be null.");
            }
            Product product = new Product
            {
                
                Name = productInfo.Name,
                ImageUrl = productInfo.ImageUrl,
                Price = productInfo.Price,
                Stock = productInfo.Stock,
                CreatedAt = DateTime.Now
            };
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
