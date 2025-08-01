using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Stok.Worker.Data;

namespace Stok.Worker.Consumers
{
    public class StokAyirCommandConsumer : IConsumer<StokAyirCommand>
    {
        private readonly ProductContext _context;
        private readonly ILogger<StokAyirCommandConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StokAyirCommandConsumer(ProductContext context, ILogger<StokAyirCommandConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StokAyirCommand> context)
        {
            var command = context.Message;
            _logger.LogInformation("{CorrelationId} ID'li sipariş için stok ayırma işlemi başlatıldı.", command.CorrelationId);

            var yetersizUrunler = new List<int>();

            
            foreach (var item in command.SiparisKalemleri)
            {
                var productStock = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.UrunId);
                if (productStock == null || productStock.StockQuantity < item.Adet)
                {
                    yetersizUrunler.Add(item.UrunId);
                }
            }
           
            if (yetersizUrunler.Any())
            {
                
                await _publishEndpoint.Publish(new StokYetersizEvent
                {
                    CorrelationId = command.CorrelationId,
                    YetersizUrunIdleri = yetersizUrunler
                });
                _logger.LogWarning("{CorrelationId} ID'li sipariş için stok yetersiz. Ürün ID'leri: {Urunler}", command.CorrelationId, string.Join(",", yetersizUrunler));
            }
            else 
            {
                
                foreach (var item in command.SiparisKalemleri)
                {
                    var productStock = await _context.Products.FirstAsync(p => p.Id == item.UrunId);
                    productStock.StockQuantity -= item.Adet;
                }
                await _context.SaveChangesAsync();

                
                await _publishEndpoint.Publish(new StokAyrildiEvent { CorrelationId = command.CorrelationId });
                _logger.LogInformation("{CorrelationId} ID'li sipariş için stok başarıyla ayrıldı.", command.CorrelationId);
            }
        }
    }

}
