using ShippingService.Data;
using ShippingService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShippingService.Services
{
    public class ShippingServices : IShippingServices
    {
        private readonly ShippingContext _context;

        public ShippingServices(ShippingContext context)
        {
            _context = context;
        }

        public async Task<Shipping> CreateShippingAsync(Guid customerId, Guid orderId, string? addressId = null)
        {
            var shipping = new Shipping
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                OrderId = orderId,
                AddressId = addressId,
                Status = ShippingStatus.Hazirlaniyor,
                ShippedAt = DateTime.UtcNow
            };

            _context.Shippings.Add(shipping);
            await _context.SaveChangesAsync();
            return shipping;
        }

        public async Task<Shipping?> GetShippingByOrderIdAsync(Guid orderId)
        {
            return await _context.Shippings.FirstOrDefaultAsync(s => s.OrderId == orderId);
        }

        public async Task<bool> UpdateShippingStatusAsync(Guid orderId, ShippingStatus newStatus)
        {
            var shipping = await _context.Shippings.FirstOrDefaultAsync(s => s.OrderId == orderId);
            if (shipping == null) return false;

            shipping.Status = newStatus;

            if (newStatus == ShippingStatus.KargoyaVerildi)
                shipping.ShippedAt = DateTime.UtcNow;

            if (newStatus == ShippingStatus.TeslimEdildi)
                shipping.DeliveredAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CancelShippingAsync(Guid orderId)
        {
            var shipping = await _context.Shippings.FirstOrDefaultAsync(s => s.OrderId == orderId);
            if (shipping == null) return false;

            shipping.Status = ShippingStatus.IptalEdildi;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Shipping>> GetShippingByCustomerIdAsync(Guid customerId)
        {
            return await _context.Shippings.Where(s => s.CustomerId == customerId).ToListAsync();
        }




    }
}
