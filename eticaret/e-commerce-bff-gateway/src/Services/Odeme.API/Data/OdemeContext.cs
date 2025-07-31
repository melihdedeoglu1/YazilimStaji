using Microsoft.EntityFrameworkCore;
using Odeme.API.Models;

namespace Odeme.API.Data
{
    public class OdemeContext : DbContext
    {
        public OdemeContext(DbContextOptions<OdemeContext> options) : base(options) { }

        public DbSet<OdemeKaydi> OdemeKayitlari { get; set; }


    }
}
