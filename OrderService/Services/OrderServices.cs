using OrderService.DTOs;
using OrderService.Models;
using System;
using OrderService.Data;
namespace OrderService.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly OrderContext _context;

        public OrderServices(OrderContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto dto)
        {
           
            var orderItems = dto.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList();

            
            var order = new Order
            {
                Id = Guid.NewGuid(), 
                CustomerId = dto.CustomerId,
                CreatedAt = DateTime.UtcNow,
                Items = orderItems 
            };

           
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
