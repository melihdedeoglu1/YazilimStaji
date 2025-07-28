using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts; 
using System; 
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

            

            
            _logger.LogInformation(
                "--> {SiparisId} ID'li sipariş için e-posta ya da herhangi bir şey simüle etmek istediğimde burayı kullanacağım.",
                eventMessage.SiparisId);

            return Task.CompletedTask;
        }
    }
}