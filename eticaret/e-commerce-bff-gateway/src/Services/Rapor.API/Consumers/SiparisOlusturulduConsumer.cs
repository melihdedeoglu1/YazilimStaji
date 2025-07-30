using MassTransit;
using Rapor.API.Data;
using Rapor.API.Models;
using Shared.Contracts; 
using Microsoft.EntityFrameworkCore;

namespace Rapor.API.Consumers
{
    public class SiparisOlusturulduConsumer : IConsumer<SiparisOlusturulduEvent>
    {

        private readonly RaporContext _context;
        private readonly ILogger<SiparisOlusturulduConsumer> _logger;

        public SiparisOlusturulduConsumer(RaporContext context, ILogger<SiparisOlusturulduConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SiparisOlusturulduEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Sipariş Oluşturuldu eventi işleniyor: Sipariş ID {SiparisId}", message.SiparisId);

            
            var musteriRaporu = await _context.MusteriSiparisRaporlari.FirstOrDefaultAsync(m => m.UserId == message.KullaniciId);
            if (musteriRaporu == null)
            {
                musteriRaporu = new MusteriSiparisRaporu
                {
                    UserId = message.KullaniciId,
                    UserEmail = message.KullaniciEmail,
                    UserName =message.KullaniciAdi, 
                    UserCreatedDate = message.UserDate,
                    OrderCount = 1,
                    RefundCount = 0 
                };
                await _context.MusteriSiparisRaporlari.AddAsync(musteriRaporu);
            }
            else
            {
                musteriRaporu.OrderCount++;
            }


            
            foreach (var item in message.SiparisKalemleri)
            {
                
                var urunPerformans = await _context.UrunPerformansRaporlari.FirstOrDefaultAsync(u => u.ProductId == item.UrunId);
                if (urunPerformans == null)
                {
                    urunPerformans = new UrunPerformansRaporu
                    {
                        ProductId = item.UrunId,
                        ProductName = item.UrunAdi,
                        OrderedCount = item.Adet,
                        ProductCreatedDate = DateTime.UtcNow,
                        RefundedCount = 0 
                    };
                    await _context.UrunPerformansRaporlari.AddAsync(urunPerformans);
                }
                else
                {
                    urunPerformans.OrderedCount += item.Adet;
                }

                
                var siparisDetay = new SiparisDetayRaporu
                {
                    OrderId = message.SiparisId,
                    UserId = message.KullaniciId,
                    UserEmail = message.KullaniciEmail,
                    UserRole = message.KullaniciRol,
                    OrderDate = message.SiparisTarihi,
                    TotalPrice = message.ToplamTutar,
                    OrderStatus = message.Durum,
                    ProductName = item.UrunAdi,
                    ProductPrice = item.Fiyat,
                    ProductQuantity = item.Adet
                };
                await _context.SiparisDetayRaporlari.AddAsync(siparisDetay);
            }

            
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _logger.LogInformation("Sipariş Oluşturuldu eventi başarıyla işlendi. Veritabanında {Count} değişiklik yapıldı.", result);
            }
        }

    }
}
