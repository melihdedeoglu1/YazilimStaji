using OrderService.DTOs;
using OrderService.Models;



namespace OrderService.Services
{
    public interface IOrderServices
    {
        Task<Order> CreateOrderAsync(CreateOrderDto createOrderDto);       
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<bool> CancelOrderAsync(Guid orderId);
    }
}
