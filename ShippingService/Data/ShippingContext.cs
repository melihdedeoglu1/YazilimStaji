using Microsoft.EntityFrameworkCore;

namespace ShippingService.Data
{
    public class ShippingContext : DbContext
    {
        public ShippingContext(DbContextOptions<ShippingContext> options) : base(options)
        {
        }
        public DbSet<Models.Shipping> Shippings { get; set; } = null!;
    }
    
}
