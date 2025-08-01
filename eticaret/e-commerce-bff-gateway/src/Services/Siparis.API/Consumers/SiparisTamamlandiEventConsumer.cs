using MassTransit;
using Shared.Contracts;
using Siparis.API.Data;
using Siparis.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Siparis.API.Consumers
{
    public class SiparisTamamlandiEventConsumer : IConsumer<SiparisTamamlandiEvent>
    {
        private readonly SiparisContext _siparisContext;
        private readonly ILogger<SiparisTamamlandiEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public SiparisTamamlandiEventConsumer(SiparisContext siparisContext, ILogger<SiparisTamamlandiEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _siparisContext = siparisContext;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<SiparisTamamlandiEvent> context)
        {
           
            var message = context.Message;

            var newOrder = new Order
            {
                UserId = message.KullaniciId,
                TotalPrice = message.ToplamTutar,
                OrderDate = message.TalepTarihi,
                Status = "Tamamlandı",
                OrderItems = message.SiparisKalemleri.Select(item => new OrderItem
                {
                    ProductId = item.UrunId,
                    ProductName = item.UrunAdi,
                    Price = item.Fiyat,
                    Quantity = item.Adet
                }).ToList()
            };

            await _siparisContext.Orders.AddAsync(newOrder);
            await _siparisContext.SaveChangesAsync();
            _logger.LogInformation("Sipariş başarıyla oluşturuldu. SiparisId: {SiparisId}", newOrder.Id);

            
            var eventMessage = new SiparisOlusturulduEvent
            {
                SiparisId = newOrder.Id,
                KullaniciId = message.KullaniciId,
                KullaniciEmail = message.KullaniciEmail,
                ToplamTutar = message.ToplamTutar,
                SiparisTarihi = newOrder.OrderDate,
                Durum = newOrder.Status,
                KullaniciRol = message.KullaniciRol,
                KullaniciAdi = message.KullaniciAdi,
                UserDate = message.UserDate,
                SiparisKalemleri = message.SiparisKalemleri.Select(item => new SiparisKalemiDto
                {
                    UrunId = item.UrunId,
                    UrunAdi = item.UrunAdi,
                    Fiyat = item.Fiyat,
                    Adet = item.Adet
                }).ToList()
            };

            await _publishEndpoint.Publish(eventMessage);
        }
    }
}