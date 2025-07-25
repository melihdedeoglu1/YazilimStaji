using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Siparis.API.Data;

namespace Siparis.API.Consumers
{
    public class StokGuncellemeBasarisizConsumer : IConsumer<StokGuncellemeBasarisizEvent>
    {
        private readonly ILogger<StokGuncellemeBasarisizConsumer> _logger;

        private readonly SiparisContext _context;


        public StokGuncellemeBasarisizConsumer(ILogger<StokGuncellemeBasarisizConsumer> logger, SiparisContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Consume(ConsumeContext<StokGuncellemeBasarisizEvent> context)
        {
            var basarisizlikEvent = context.Message;
            _logger.LogWarning("Stok hatası telafi işlemi başlatıldı. SiparisId: {SiparisId}, Gerekçe: {Gerekçe}",
                    basarisizlikEvent.SiparisId, basarisizlikEvent.Gerekce);

            var siparis = await _context.Orders.FirstOrDefaultAsync(s => s.Id == basarisizlikEvent.SiparisId);

            if (siparis == null)
            {
                _logger.LogError("Telafi işlemi başarısız: Sipariş bulunamadı! SiparisId: {SiparisId}", basarisizlikEvent.SiparisId);
                return;
            }

            
            siparis.Status = "İptal Edildi - Stok Yetersiz"; 

            await _context.SaveChangesAsync();

            _logger.LogInformation("Sipariş başarıyla iptal edildi. SiparisId: {SiparisId}", siparis.Id);
        }


    }
}
