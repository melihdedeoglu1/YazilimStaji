using Microsoft.EntityFrameworkCore;
using Rapor.API.Models;

namespace Rapor.API.Data
{
    public class RaporContext : DbContext
    {
       public RaporContext(DbContextOptions<RaporContext> options) : base(options) { }

        public DbSet<MusteriSiparisRaporu> MusteriSiparisRaporlari {  get; set; }

        public DbSet<SiparisDetayRaporu> SiparisDetayRaporlari { get; set; }

        public DbSet<UrunPerformansRaporu> UrunPerformansRaporlari { get; set; }
        
    }
}
