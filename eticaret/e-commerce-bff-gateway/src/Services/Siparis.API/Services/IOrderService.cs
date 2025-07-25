using Siparis.API.DTOs;
using Siparis.API.Models;

namespace Siparis.API.Services
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(int userId, string userEmail, OrderForCreateDto orderDto);

        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);

    }
}
