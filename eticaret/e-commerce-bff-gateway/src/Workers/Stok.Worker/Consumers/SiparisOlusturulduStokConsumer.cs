using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Stok.Worker.Data;
using Stok.Worker.Models;

namespace Stok.Worker.Consumers
{
    public class SiparisOlusturulduStokConsumer : IConsumer<SiparisOlusturulduEvent>
    {
        private readonly ILogger<SiparisOlusturulduStokConsumer> _logger;
        private readonly ProductContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        public SiparisOlusturulduStokConsumer(ILogger<SiparisOlusturulduStokConsumer> logger, ProductContext context, IPublishEndpoint publishEndpoint) 
        {
            _logger = logger;
            _context = context;
            _publishEndpoint = publishEndpoint;

        }


        public async Task Consume(ConsumeContext<SiparisOlusturulduEvent> context) 
        {
            var siparisEvent = context.Message;
            var gecerliUrunler = new List<Product>();

            _logger.LogInformation("Veritabanı stok güncelleme işlemi başladı. SiparisId: {SiparisId}", siparisEvent.SiparisId);

            _logger.LogInformation(
                    "Stok güncelleme talebi geldi. SiparisId: {SiparisId}, Toplam Tutar: {ToplamTutar}",
                    siparisEvent.SiparisId,
                    siparisEvent.ToplamTutar
                );

            foreach (var item in siparisEvent.SiparisKalemleri)
            {
                var urun = await _context.Products.FirstOrDefaultAsync(u => u.Id == item.UrunId);
                if (urun == null)
                {
                    _logger.LogWarning("STOK GÜNCELLENEMEDİ: Ürün bulunamadı! SiparisId: {SiparisId}, UrunId: {UrunId}",
                        siparisEvent.SiparisId, item.UrunId);

                    return;
                }

                if (urun.StockQuantity < item.Adet)
                {
                    _logger.LogError("Yetersiz stok! SiparisId: {SiparisId}, UrunId: {UrunId}. Telafi işlemi başlatılıyor.", siparisEvent.SiparisId, item.UrunId);

                    await _publishEndpoint.Publish(new StokGuncellemeBasarisizEvent
                    {
                        SiparisId = siparisEvent.SiparisId,
                        Gerekce = $"'{item.UrunAdi}' için stok yetersiz. Gerekli: {item.Adet}, Mevcut: {urun.StockQuantity}"
                    });
                    return; 
                }
                gecerliUrunler.Add(urun);
            }

            _logger.LogInformation("Tüm ürünlerin stok kontrolü başarılı. Güncelleme işlemi başlıyor. SiparisId: {SiparisId}", siparisEvent.SiparisId);

            for (int i = 0; i < gecerliUrunler.Count; i++)
            {
                var urun = gecerliUrunler[i];
                var siparisKalemi = siparisEvent.SiparisKalemleri[i];

                urun.StockQuantity -= siparisKalemi.Adet;

                _logger.LogInformation("Stok güncellendi. UrunId: {UrunId}, Yeni Stok: {YeniStok}",
                    siparisKalemi.UrunId, urun.StockQuantity);
            }
            await _context.SaveChangesAsync();

            _logger.LogInformation("Veritabanı stok güncelleme işlemi başarıyla tamamlandı. SiparisId: {SiparisId}", siparisEvent.SiparisId);

            

        }



    }
}
