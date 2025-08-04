using MassTransit;
using Kargo.API.Data; 
using Kargo.API.Models; 
using Shared.Contracts;

namespace Kargo.API.Consumers
{
    public class KargoHazirlaCommandConsumer : IConsumer<KargoHazirlaCommand>
    {
        private readonly KargoContext _context;
        private readonly ILogger<KargoHazirlaCommandConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public KargoHazirlaCommandConsumer(KargoContext context, ILogger<KargoHazirlaCommandConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<KargoHazirlaCommand> context)
        {
            var command = context.Message;
            _logger.LogInformation("{CorrelationId} ID'li sipariş için kargo hazırlama işlemi başlatıldı.", command.CorrelationId);

            
            var kargoGonderi = new KargoGonderi
            {
                SiparisId = command.SiparisId,
                KullaniciId = command.KullaniciId,
                Durum = "Hazırlanıyor",
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _context.KargoKayitlari.AddAsync(kargoGonderi);
            await _context.SaveChangesAsync();

            

            await _publishEndpoint.Publish(new KargoHazirlandiEvent { CorrelationId = command.CorrelationId });
            _logger.LogInformation("{CorrelationId} ID'li sipariş için kargo kaydı başarıyla oluşturuldu.", command.CorrelationId);



        }
    }
}
