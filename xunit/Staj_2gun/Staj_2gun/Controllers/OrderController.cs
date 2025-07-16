using Microsoft.AspNetCore.Mvc;
using Staj_2gun.Models;
using Staj_2gun.Services;

namespace Staj_2gun.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _orderService.GetAll();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _orderService.GetById(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public IActionResult Add(Order order)
        {
            _orderService.Add(order);
            return Ok();
        }

        [HttpGet("details/{customerId}")]
        public IActionResult GetOrderDetailsByCustomer(int customerId)
        {
            var data = _orderService.GetCustomerOrderDetails(customerId);
            return Ok(data);
        }

    }
}
