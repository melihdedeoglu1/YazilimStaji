using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShippingService.DTOs;
using ShippingService.Models;
using ShippingService.Services;

namespace ShippingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingServices _shippingServices;

        public ShippingController(IShippingServices shippingServices)
        {
            _shippingServices = shippingServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipping([FromBody] ShippingCreateRequestDto request)
        {
            var shipping = await _shippingServices.CreateShippingAsync(request.CustomerId, request.OrderId, request.AddressId);
            return CreatedAtAction(nameof(GetShippingByOrderId), new { orderId = shipping.OrderId }, shipping);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetShippingByOrderId(Guid orderId)
        {
            var shipping = await _shippingServices.GetShippingByOrderIdAsync(orderId);
            if (shipping == null)
                return NotFound("Kargo bilgisi bulunamadı.");

            return Ok(shipping);
        }

        [HttpPatch("{orderId}/status")]
        public async Task<IActionResult> UpdateShippingStatus(Guid orderId, [FromQuery] ShippingStatus status)
        {
            var result = await _shippingServices.UpdateShippingStatusAsync(orderId, status);
            if (!result)
                return NotFound("Sipariş için kargo bilgisi bulunamadı.");

            return NoContent(); // Başarılı ama içerik dönmüyoruz
        }

        [HttpPatch("cancel/{orderId}")]
        public async Task<IActionResult> CancelShipping(Guid orderId)
        {
            var success = await _shippingServices.CancelShippingAsync(orderId);
            if (!success)
                return NotFound("Kargo bilgisi bulunamadı.");

            return Ok(new { message = "Kargo durumu iptal edildi." });
        }

    }
}
