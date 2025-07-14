using ShippingService.Models;

namespace ShippingService.Services
{
    public interface IShippingServices
    {
        Task<Shipping> CreateShippingAsync(Guid customerId, Guid orderId, string? addressId = null);
        Task<Shipping?> GetShippingByOrderIdAsync(Guid orderId);
        Task<bool> UpdateShippingStatusAsync(Guid orderId, ShippingStatus newStatus);
        Task<bool> CancelShippingAsync(Guid orderId);
    }
}
