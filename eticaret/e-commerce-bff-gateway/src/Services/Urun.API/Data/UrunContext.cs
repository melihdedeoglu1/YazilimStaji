using Microsoft.EntityFrameworkCore;
using Urun.API.Models;
namespace Urun.API.Data
{
    public class UrunContext : DbContext
    {
        public UrunContext(DbContextOptions<UrunContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        

    }
}
