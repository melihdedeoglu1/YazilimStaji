using UnitTestOrnek.Data;
using Microsoft.EntityFrameworkCore;
using UnitTestOrnek.Models;

namespace UnitTestOrnek.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Context _context;

        public OrderRepository(Context context) 
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync() 
        {
            return await _context.Orders.ToListAsync();      
        }

        public async Task<Order?> GetByIdAsync(int id) 
        { 
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order?> GetOrderWithCustomerByIdAsync(int id) 
        { 
            return await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateAsync(Order order) 
        {
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<bool> UpdateAsync(int id, Order updatedOrder) 
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null) return false;

            order.ProductName = updatedOrder.ProductName;
            order.Total = updatedOrder.Total;
            order.CustomerId = updatedOrder.CustomerId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null) return false;

            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return true;
        }


    }
}
