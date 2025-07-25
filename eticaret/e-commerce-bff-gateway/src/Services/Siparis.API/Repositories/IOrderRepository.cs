using Siparis.API.Models;

namespace Siparis.API.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);

        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);

    }
}
