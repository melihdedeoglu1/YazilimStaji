using Microsoft.EntityFrameworkCore;
using Siparis.API.Models;

namespace Siparis.API.Data
{
    public class SiparisContext : DbContext
    {
        public SiparisContext(DbContextOptions<SiparisContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems) 
                .WithOne(oi => oi.Order)    
                .HasForeignKey(oi => oi.OrderId); 
        }

    }
}
