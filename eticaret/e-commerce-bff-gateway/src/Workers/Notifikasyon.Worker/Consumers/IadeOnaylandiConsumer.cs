using MassTransit;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifikasyon.Worker.Consumers
{
    public class IadeOnaylandiConsumer : IConsumer<IadeOnaylandiEvent>
    {
        private readonly ILogger<IadeOnaylandiConsumer> _logger;

        public IadeOnaylandiConsumer(ILogger<IadeOnaylandiConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IadeOnaylandiEvent> context)
        {
            var eventMessage = context.Message;

            _logger.LogInformation(
                "--> Yeni Sipariş İade Mesajı Alındı! SiparisId: {SiparisId}, Kullanıcı: {KullaniciEmail} ! Siparişiniz iptal ediliyor.",
                eventMessage.OrderId,
                eventMessage.UserId);


            _logger.LogInformation(
                "--> {SiparisId} ID'li sipariş için e-posta ya da herhangi bir şey simüle etmek istediğimde burayı kullanacağım.",
                eventMessage.OrderId);

            return Task.CompletedTask;
        }

    }
}
