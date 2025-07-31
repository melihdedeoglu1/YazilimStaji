using Microsoft.EntityFrameworkCore;
using Kargo.API.Models;


namespace Kargo.API.Data
{
    public class KargoContext : DbContext
    {
        public KargoContext(DbContextOptions<KargoContext> options) : base(options) { }

        public DbSet<KargoGonderi> KargoKayitlari { get; set; }

    }
}
