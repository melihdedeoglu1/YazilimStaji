using Microsoft.EntityFrameworkCore;
using Staj_2gun.Models;

namespace Staj_2gun.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CUSTOMER
            modelBuilder.Entity<Customer>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Customer silinince orders da silinsin

            // PRODUCT
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Stock)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasCheckConstraint("CK_Product_Stock_Positive", "[Stock] >= 0");

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Ürün silinemez, önce ilişkili orderitems silinmeli

            // ORDER
            modelBuilder.Entity<Order>()
                .Property(o => o.CustomerId)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Order silinince orderitems da silinsin

            // ORDERITEM
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Quantity)
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .HasCheckConstraint("CK_OrderItem_Quantity_Positive", "[Quantity] > 0");
        }


    }
}
