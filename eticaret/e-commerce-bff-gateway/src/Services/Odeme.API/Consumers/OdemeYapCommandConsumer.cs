using MassTransit;
using Odeme.API.Data;
using Odeme.API.Models;
using Shared.Contracts;
using System.Text;
using System.Text.Json;
namespace Odeme.API.Consumers
{
    public class OdemeYapCommandConsumer : IConsumer <OdemeYapCommand>
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OdemeYapCommandConsumer> _logger;
        private readonly OdemeContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public OdemeYapCommandConsumer(IHttpClientFactory httpClientFactory, ILogger<OdemeYapCommandConsumer> logger, OdemeContext context, IPublishEndpoint publishEndpoint)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OdemeYapCommand> context)
        {
            var command = context.Message;
            _logger.LogInformation("{CorrelationId} ID'li sipariş için ödeme işlemi başlatıldı.", command.CorrelationId);

            bool bakiyeYeterliMi = false;
            try
            {
                var client = _httpClientFactory.CreateClient();
               
                var content = new StringContent(JsonSerializer.Serialize(new { Tutar = command.Tutar }), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://kullanici-servisi:8080/api/user/{command.KullaniciId}/bakiye-dusur", content);

                if (response.IsSuccessStatusCode)
                {
                    bakiyeYeterliMi = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanici.API'ye bakiye düşürme isteği atılırken hata oluştu.");
            }

            var odemeKaydi = new OdemeKaydi
            {
                CorrelationId = command.CorrelationId, 
                KullaniciId = command.KullaniciId,
                Tutar = command.Tutar,
                OdemeTarihi = DateTime.UtcNow,
                Durum = bakiyeYeterliMi ? "Basarili" : "Basarisiz"
            };
            await _context.OdemeKayitlari.AddAsync(odemeKaydi);
            await _context.SaveChangesAsync();

            if (bakiyeYeterliMi)
            {
                await _publishEndpoint.Publish(new OdemeBasariliEvent { CorrelationId = command.CorrelationId });
                _logger.LogInformation("{CorrelationId} ID'li sipariş için ödeme başarılı.", command.CorrelationId);
            }
            else
            {
                await _publishEndpoint.Publish(new OdemeBasarisizEvent
                {
                    CorrelationId = command.CorrelationId,
                    HataMesaji = "Yetersiz bakiye."
                });
                _logger.LogWarning("{CorrelationId} ID'li sipariş için ödeme başarısız: Yetersiz bakiye.", command.CorrelationId);
            }
        }

    }
}
