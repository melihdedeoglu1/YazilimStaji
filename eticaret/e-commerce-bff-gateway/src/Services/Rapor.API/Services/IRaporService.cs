using Rapor.API.Models;

namespace Rapor.API.Services
{
    public interface IRaporService
    {
        Task<IEnumerable<UrunPerformansRaporu>> GetBestSellingProductsAsync(int count);
        Task<IEnumerable<UrunPerformansRaporu>> GetMostRefundedProductsAsync(int count);
        Task<IEnumerable<MusteriSiparisRaporu>> GetTopCustomersAsync(int count);
        Task<IEnumerable<SiparisDetayRaporu>> GetRecentOrdersAsync(int count);
    }
}
