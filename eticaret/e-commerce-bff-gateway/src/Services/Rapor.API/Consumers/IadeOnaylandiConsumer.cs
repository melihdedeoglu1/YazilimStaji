using MassTransit;
using Rapor.API.Data;
using Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Rapor.API.Consumers
{
    public class IadeOnaylandiConsumer : IConsumer<IadeOnaylandiEvent>
    {
        private readonly RaporContext _context;
        private readonly ILogger<IadeOnaylandiConsumer> _logger;

        public IadeOnaylandiConsumer(RaporContext context, ILogger<IadeOnaylandiConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IadeOnaylandiEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Iade Onaylandi eventi işleniyor: Sipariş ID {SiparisId}", message.OrderId);

            
            var musteriRaporu = await _context.MusteriSiparisRaporlari.FirstOrDefaultAsync(m => m.UserId == message.UserId);
            if (musteriRaporu != null)
            {
                
                musteriRaporu.RefundCount++;
            }
            else
            {
                _logger.LogWarning("İade yapılan müşteri raporu bulunamadı. UserId: {UserId}", message.UserId);
            }

            
            foreach (var iadeEdilenUrun in message.Products)
            {
                var urunPerformans = await _context.UrunPerformansRaporlari.FirstOrDefaultAsync(u => u.ProductId == iadeEdilenUrun.ProductId);

                if (urunPerformans != null)
                {
                   
                    urunPerformans.RefundedCount += iadeEdilenUrun.Quantity;
                }
                else
                {
                    _logger.LogWarning("İade edilen ürün performans raporu bulunamadı. ProductId: {ProductId}", iadeEdilenUrun.ProductId);
                }
            }

            
            await _context.SaveChangesAsync();
            _logger.LogInformation("Iade Onaylandi eventi başarıyla işlendi.");
        }

    }
}
