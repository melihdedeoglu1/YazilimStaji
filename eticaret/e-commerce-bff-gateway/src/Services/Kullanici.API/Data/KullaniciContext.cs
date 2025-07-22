using Microsoft.EntityFrameworkCore;
using Kullanici.API.Models;

namespace Kullanici.API.Data
{
    public class KullaniciContext : DbContext
    {
        public KullaniciContext(DbContextOptions<KullaniciContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    } 
}

