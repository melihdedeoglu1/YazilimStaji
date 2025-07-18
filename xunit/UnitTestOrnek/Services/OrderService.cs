using AutoMapper;
using UnitTestOrnek.Repositories;
using UnitTestOrnek.Models;
using UnitTestOrnek.DTOs;

namespace UnitTestOrnek.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }   

        public async Task<Order> GetOrderWithCustomerByIdAsync(int id)
        {
            return await _orderRepository.GetOrderWithCustomerByIdAsync(id);
        }

        public async Task<Order> CreateOrderAsync(OrderDto dto)
        {
            var order = _mapper.Map<Order>(dto);
            return await _orderRepository.CreateAsync(order);
        }

        public async Task<bool> UpdateOrderAsync(int id, OrderDto dto)
        {
            var updatedOrder = _mapper.Map<Order>(dto);
            updatedOrder.Id = id;
            return await _orderRepository.UpdateAsync(id, updatedOrder);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

    }
}
