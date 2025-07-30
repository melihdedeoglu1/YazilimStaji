using Microsoft.EntityFrameworkCore;
using Rapor.API.Data;
using Rapor.API.Models;

namespace Rapor.API.Repositories
{
    public class RaporRepository : IRaporRepository
    {
        private readonly RaporContext _context;

        public RaporRepository(RaporContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UrunPerformansRaporu>> GetBestSellingProductsAsync(int count)
        {
            return await _context.UrunPerformansRaporlari
                .AsNoTracking()
                .OrderByDescending(p => p.OrderedCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<UrunPerformansRaporu>> GetMostRefundedProductsAsync(int count)
        {
            return await _context.UrunPerformansRaporlari
                .AsNoTracking()
                .Where(p => p.RefundedCount > 0)
                .OrderByDescending(p => p.RefundedCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<MusteriSiparisRaporu>> GetTopCustomersAsync(int count)
        {
            return await _context.MusteriSiparisRaporlari
                .AsNoTracking()
                .OrderByDescending(c => c.OrderCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<SiparisDetayRaporu>> GetRecentOrdersAsync(int count)
        {
            return await _context.SiparisDetayRaporlari
                .AsNoTracking()
                .OrderByDescending(d => d.OrderDate)
                .Take(count)
                .ToListAsync();
        }
    }
}