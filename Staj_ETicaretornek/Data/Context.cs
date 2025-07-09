using Microsoft.EntityFrameworkCore;
using Staj_ETicaretornek.Models;

namespace Staj_ETicaretornek.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Product>().ToTable("Products");
            base.OnModelCreating(modelBuilder);



        }
    
    
    }
}
