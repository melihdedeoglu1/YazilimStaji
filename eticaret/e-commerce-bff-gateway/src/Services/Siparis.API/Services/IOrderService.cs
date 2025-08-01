﻿using Siparis.API.DTOs;
using Siparis.API.Models;

namespace Siparis.API.Services
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderSagaAsync(int userId, string userEmail, string role, string userName, OrderForCreateDto orderDto);
        Task<Order> CreateAsync(int userId, string userEmail,string role,string userName, OrderForCreateDto orderDto);

        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);

    }
}
