using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        /*
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null || createOrderDto.Items == null || !createOrderDto.Items.Any())
            {
                return BadRequest("Invalid order data.");
            }
            var order = await _orderServices.CreateOrderAsync(createOrderDto);
            return CreatedAtAction(nameof(CreateOrderAsync), new { id = order.Id }, order);

        }
        */
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null)
                return BadRequest("createOrderDto is null");

            var result = await _orderServices.CreateOrderAsync(createOrderDto);
            return Ok(new { orderId = result.Id });
        }
    }
}
