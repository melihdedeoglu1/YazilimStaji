using OrderService.DTOs;
using OrderService.Models;
using System;
using OrderService.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return false;

            // Önce ilişkili OrderItem'ları sil
            _context.OrderItems.RemoveRange(order.Items);

            // Sonra Order'ı sil
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
