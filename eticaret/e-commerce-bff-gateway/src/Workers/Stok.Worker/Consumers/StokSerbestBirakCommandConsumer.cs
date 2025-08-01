using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Stok.Worker.Data;

namespace Stok.Worker.Consumers
{
    public class StokSerbestBirakCommandConsumer : IConsumer<StokSerbestBirakCommand>
    {
        private readonly ProductContext _context;
        private readonly ILogger<StokSerbestBirakCommandConsumer> _logger;

        public StokSerbestBirakCommandConsumer(ProductContext context, ILogger<StokSerbestBirakCommandConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StokSerbestBirakCommand> context)
        {
            var command = context.Message;
            _logger.LogInformation("{CorrelationId} ID'li siparişin iptali nedeniyle stoklar serbest bırakılıyor.", command.CorrelationId);

            foreach (var item in command.SiparisKalemleri)
            {
                var productStock = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.UrunId);
                if (productStock != null)
                {                  
                    productStock.StockQuantity += item.Adet;
                }
            }
            await _context.SaveChangesAsync();

            _logger.LogInformation("{CorrelationId} ID'li sipariş için stoklar başarıyla serbest bırakıldı.", command.CorrelationId);
        }
    }
}
