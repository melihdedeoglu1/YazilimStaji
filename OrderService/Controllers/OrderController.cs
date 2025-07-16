using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.DTOs;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null)
                return BadRequest("createOrderDto is null");

            var result = await _orderServices.CreateOrderAsync(createOrderDto);
            return Ok(new { orderId = result.Id });
        }

        [HttpGet("{id}/total")]
        public async Task<IActionResult> GetOrderTotalById(Guid id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            var totalAmount = order.Items.Sum(item => item.Price * item.Quantity);

            return Ok(new
            {
                order.Id,
                order.CustomerId,               
                totalAmount
            });
        }

        [HttpPost("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var result = await _orderServices.CancelOrderAsync(orderId);
            if (!result)
                return NotFound("Sipariş bulunamadı veya zaten silinmiş.");

            return Ok(new { message = "Sipariş iptal edildi." });
        }


        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetOrderDetails(Guid id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound("Sipariş bulunamadı.");

            var dto = new OrderDetailsDto
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                Items = order.Items.Select(i => new CreateOrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            return Ok(dto);
        }

    }
}
