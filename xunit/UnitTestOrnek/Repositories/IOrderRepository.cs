using UnitTestOrnek.Models;

namespace UnitTestOrnek.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetOrderWithCustomerByIdAsync(int id);
        Task<Order> CreateAsync(Order order);
        Task<bool> UpdateAsync(int id, Order updatedOrder);
        Task<bool> DeleteAsync(int id);
    }
}
