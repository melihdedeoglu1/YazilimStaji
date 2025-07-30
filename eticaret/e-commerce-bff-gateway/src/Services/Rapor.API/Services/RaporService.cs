
using Rapor.API.Models;
using Rapor.API.Repositories;

namespace Rapor.API.Services
{
    public class RaporService : IRaporService
    {
        private readonly IRaporRepository _raporRepository;

        public RaporService(IRaporRepository raporRepository)
        {
            _raporRepository = raporRepository;
        }

        public async Task<IEnumerable<UrunPerformansRaporu>> GetBestSellingProductsAsync(int count)
        {
            
            return await _raporRepository.GetBestSellingProductsAsync(count);
        }

        public async Task<IEnumerable<UrunPerformansRaporu>> GetMostRefundedProductsAsync(int count)
        {
            return await _raporRepository.GetMostRefundedProductsAsync(count);
        }

        public async Task<IEnumerable<MusteriSiparisRaporu>> GetTopCustomersAsync(int count)
        {
            return await _raporRepository.GetTopCustomersAsync(count);
        }

        public async Task<IEnumerable<SiparisDetayRaporu>> GetRecentOrdersAsync(int count)
        {
            return await _raporRepository.GetRecentOrdersAsync(count);
        }
    }
}