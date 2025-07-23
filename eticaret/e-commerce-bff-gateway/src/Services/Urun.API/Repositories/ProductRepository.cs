using Urun.API.Data;
using Urun.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Urun.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly UrunContext _context;
        public ProductRepository(UrunContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product product) 
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return null; 
            }

            
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingProduct;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var productToDelete = await _context.Products.FindAsync(id);
            if (productToDelete == null)
            {
                return false; 
            }

            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();
            return true; 
        }


    }
}
