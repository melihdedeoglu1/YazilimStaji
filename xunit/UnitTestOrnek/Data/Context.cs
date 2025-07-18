using Microsoft.EntityFrameworkCore;
using UnitTestOrnek.Models;
namespace UnitTestOrnek.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }


    }
}
