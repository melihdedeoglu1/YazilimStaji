using UnitTestOrnek.Models;
using UnitTestOrnek.DTOs;

namespace UnitTestOrnek.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order?> GetOrderWithCustomerByIdAsync(int id);
        Task<Order> CreateOrderAsync(OrderDto dto);
        Task<bool> UpdateOrderAsync(int id, OrderDto dto);
        Task<bool> DeleteOrderAsync(int id);
    }
}
