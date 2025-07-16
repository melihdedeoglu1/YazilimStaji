using Microsoft.EntityFrameworkCore;

namespace PaymentService.Data
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
        }

        public DbSet<Models.Payment> Payments { get; set; }

    }
}
