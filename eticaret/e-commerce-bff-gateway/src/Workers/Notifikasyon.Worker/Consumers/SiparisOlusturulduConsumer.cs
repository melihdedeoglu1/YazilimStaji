using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts; // Shared.Contracts projenizi referans aldığınızdan emin olun
using System; // InvalidOperationException için gerekli
using System.Threading.Tasks;

namespace Notifikasyon.Worker.Consumers
{
    public class SiparisOlusturulduConsumer : IConsumer<SiparisOlusturulduEvent>
    {
        private readonly ILogger<SiparisOlusturulduConsumer> _logger;

        public SiparisOlusturulduConsumer(ILogger<SiparisOlusturulduConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SiparisOlusturulduEvent> context)
        {
            var eventMessage = context.Message;

            _logger.LogInformation(
                "--> Yeni Sipariş Mesajı Alındı! SiparisId: {SiparisId}, Kullanıcı: {KullaniciEmail}",
                eventMessage.SiparisId,
                eventMessage.KullaniciEmail);

            // Hata simülasyonu: Sipariş ID'si çift sayı ise bilerek hata fırlat
            // Bu, MassTransit'in yeniden deneme (retry) mekanizmasını test etmek içindir.
            if (eventMessage.SiparisId % 2 == 1)
            {
                _logger.LogError("--> Sipariş ID {SiparisId} için bilerek hata fırlatılıyor (simülasyon).", eventMessage.SiparisId);
                throw new InvalidOperationException($"Sipariş ID {eventMessage.SiparisId} için işleme hatası oluştu.");
            }

            // Gerçek e-posta gönderme işlemi yerine simülasyon
            _logger.LogInformation(
                "--> {SiparisId} ID'li sipariş için e-posta gönderim işlemi simüle edildi.",
                eventMessage.SiparisId);

            return Task.CompletedTask;
        }
    }
}