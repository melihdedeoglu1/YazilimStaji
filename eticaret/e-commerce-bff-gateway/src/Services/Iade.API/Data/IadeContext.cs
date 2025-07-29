using Microsoft.EntityFrameworkCore;
using Iade.API.Models;

namespace Iade.API.Data
{
    public class IadeContext : DbContext
    {
        public IadeContext(DbContextOptions<IadeContext> options) : base(options) { }


        public DbSet<RefundLog> RefundLogs { get; set; }

        public DbSet<RefundProduct> RefundProducts { get; set; }

    }
}
