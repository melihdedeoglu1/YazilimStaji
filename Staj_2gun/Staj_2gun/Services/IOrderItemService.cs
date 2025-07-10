using Staj_2gun.Models;

namespace Staj_2gun.Services
{
    public interface IOrderItemService
    {
        Task<List<OrderItem>> GetAllAsync();
        Task<OrderItem?> GetByIdAsync(int id);
        Task<OrderItem> CreateAsync(OrderItem item);
        Task<OrderItem?> UpdateAsync(int id, OrderItem updatedItem);
        Task<bool> DeleteAsync(int id);
    }
}
