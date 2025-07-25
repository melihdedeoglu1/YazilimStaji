using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siparis.API.DTOs;
using Siparis.API.Models;
using Siparis.API.Services;
using System.Security.Claims;

namespace Siparis.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger) 
        {
            _orderService = orderService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult> GetMyOrders() 
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Kullanıcı kimliği bulunamadı.");
            }
            var userId = int.Parse(userIdClaim.Value);

            
            var orders = await _orderService.GetUserOrdersAsync(userId);

            if (orders == null || !orders.Any())
            {
                return Ok(new List<Order>());
            }

            return Ok(orders);
        }





        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderForCreateDto orderDto)
        {
            
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;            

            _logger.LogInformation("Creating order for User ID: {UserId}", userIdString);

            var emailClaim = User.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {              
                return Unauthorized("Geçerli bir kullanıcı e-postası bulunamadı.");
            }

            var userId = int.Parse(userIdString);
            var userEmail = emailClaim.Value;

            var createdOrder = await _orderService.CreateAsync(userId, userEmail, orderDto);

            
            return Ok(createdOrder);
        }




    }
}
