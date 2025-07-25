using Microsoft.EntityFrameworkCore;
using Siparis.API.Data;
using Siparis.API.Models;

namespace Siparis.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SiparisContext _context;

        public OrderRepository(SiparisContext context) 
        {
            _context = context;

        }

        public async Task<Order> CreateAsync(Order order)
        {           
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                                 .Include(o => o.OrderItems)
                                 .Where(o => o.UserId == userId)
                                 .OrderByDescending(o => o.OrderDate)
                                 .ToListAsync();
        }
    }
}
