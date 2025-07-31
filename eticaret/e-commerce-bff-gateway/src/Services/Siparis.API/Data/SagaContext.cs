using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Contracts;
using Siparis.API.Sagas;
using System.Text.Json;
namespace Siparis.API.Data
{
    public class SagaContext : DbContext
    {
        public SagaContext(DbContextOptions<SagaContext> options) : base(options) { }

        public DbSet<SiparisSagaState> SagaStates { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<SiparisSagaState>()
                .Property(s => s.SiparisKalemleri)
                .HasConversion(
                    
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    
                    v => JsonSerializer.Deserialize<List<SiparisKalemiSagaDto>>(v, (JsonSerializerOptions)null),
                    
                    new ValueComparer<List<SiparisKalemiSagaDto>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()));
        }
    }
}
