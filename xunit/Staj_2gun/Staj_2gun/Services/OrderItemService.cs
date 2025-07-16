using Microsoft.EntityFrameworkCore;
using Staj_2gun.Data;
using Staj_2gun.Models;

namespace Staj_2gun.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly AppDbContext _context;

        public OrderItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public async Task<OrderItem> CreateAsync(OrderItem item)
        {
            _context.OrderItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<OrderItem?> UpdateAsync(int id, OrderItem updatedItem)
        {
            var existing = await _context.OrderItems.FindAsync(id);
            if (existing == null) return null;

            existing.ProductId = updatedItem.ProductId;
            existing.OrderId = updatedItem.OrderId;
            existing.Quantity = updatedItem.Quantity;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null) return false;

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
