using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
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
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
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
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            if (roleClaim == null)
            {
                return Unauthorized("Geçerli bir kullanıcı rolü bulunamadı.");
            }
            var nameClaim = User.FindFirst(ClaimTypes.Name);
            if (nameClaim == null)
            {
                return Unauthorized("Geçerli bir kullanıcı adı bulunamadı.");
            }
            var userId = int.Parse(userIdString);
            var userEmail = emailClaim.Value;
            var role = roleClaim.Value;
            var userName = nameClaim.Value;

            var createdOrder = await _orderService.CreateAsync(userId, userEmail, role, userName, orderDto);


            return Ok(createdOrder);
        }


        [HttpPost("saga")]
        public async Task<IActionResult> CreateOrderSaga(OrderForCreateDto orderDto)
        {
            
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;            

            _logger.LogInformation("Creating order for User ID: {UserId}", userIdString);

            var emailClaim = User.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {              
                return Unauthorized("Geçerli bir kullanıcı e-postası bulunamadı.");
            }
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            if (roleClaim == null)
            {
                return Unauthorized("Geçerli bir kullanıcı rolü bulunamadı.");
            }
            var nameClaim = User.FindFirst(ClaimTypes.Name);
            if (nameClaim == null)
            {
                return Unauthorized("Geçerli bir kullanıcı adı bulunamadı.");
            }
            var userId = int.Parse(userIdString);
            var userEmail = emailClaim.Value;
            var role = roleClaim.Value;
            var userName = nameClaim.Value;

            var createdOrder = await _orderService.CreateOrderSagaAsync(userId, userEmail,role,userName, orderDto);

            
            return Ok(createdOrder);
        }

        [HttpPost("saga/cancel")] 
        public async Task<IActionResult> CancelOrderSaga([FromBody] SiparisIptalTalebiDto dto)
        {
            if (dto == null || dto.CorrelationId == Guid.Empty)
            {
                return BadRequest("Geçerli bir CorrelationId gereklidir.");
            }

            
            await _publishEndpoint.Publish(new SiparisIptalTalebiEvent
            {
                CorrelationId = dto.CorrelationId
            });

            _logger.LogInformation("{CorrelationId} ID'li sipariş süreci için iptal talebi alındı.", dto.CorrelationId);

            return Ok(new { Message = "Sipariş iptal talebiniz işleme alındı." });
        }




    }
}
