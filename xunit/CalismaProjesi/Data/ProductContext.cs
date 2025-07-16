using Microsoft.EntityFrameworkCore;

namespace CalismaProjesi.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Models.Product> Products { get; set; } 


    }
}
