using Shared.Contracts;
using MassTransit;
using Urun.API.Data;

namespace Urun.API.Consumers
{
    public class IadeOnaylandiConsumer : IConsumer<IadeOnaylandiEvent>
    {
        private readonly UrunContext _urunContext;
        private readonly ILogger<IadeOnaylandiConsumer> _logger;

        public IadeOnaylandiConsumer(UrunContext urunContext, ILogger<IadeOnaylandiConsumer> logger)
        {
            _urunContext = urunContext;
            _logger = logger;
        }


        public async Task Consume(ConsumeContext<IadeOnaylandiEvent> context)
        {
            var eventMessage = context.Message;
            _logger.LogInformation("{OrderId} numaralı siparişin ürünleri için iade onayı alındı.", eventMessage.OrderId);

            foreach (var productInEvent in eventMessage.Products)
            {
                var urun = await _urunContext.Products.FindAsync(productInEvent.ProductId);

                
                if (urun != null)
                {
                   
                    urun.StockQuantity += productInEvent.Quantity;
                    _logger.LogInformation("{OrderId} numaralı siparişin {ProductId} ID'li ürününün stok iadesi tamamlandı.", eventMessage.OrderId, productInEvent.ProductId);
                }
                else
                {
                    
                    _logger.LogWarning("Veritabanında {ProductId} ID'li ürün bulunamadı. Stok güncellenemedi.", productInEvent.ProductId);
                }
            }

            
            await _urunContext.SaveChangesAsync();

            _logger.LogInformation("Veritabanı stok güncelleme işlemi başarıyla tamamlandı. SiparisId: {SiparisId}", eventMessage.OrderId);
        }

    }
}
