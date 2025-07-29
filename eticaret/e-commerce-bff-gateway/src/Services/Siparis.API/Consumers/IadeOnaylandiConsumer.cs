using MassTransit;
using Shared.Contracts;
using Siparis.API.Data;

namespace Siparis.API.Consumers
{
    public class IadeOnaylandiConsumer : IConsumer<IadeOnaylandiEvent>
    {
        private readonly SiparisContext _context;
        private readonly ILogger<IadeOnaylandiConsumer> _logger;

        public IadeOnaylandiConsumer(SiparisContext context, ILogger<IadeOnaylandiConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IadeOnaylandiEvent> context)
        {
            var eventMessage = context.Message;
            _logger.LogInformation("{OrderId} numaralı sipariş için iade onayı alındı.", eventMessage.OrderId);

            
            var siparis = await _context.Orders.FindAsync(eventMessage.OrderId);

            if (siparis != null)
            {
                
                siparis.Status = "İade Edildi"; 

                
                await _context.SaveChangesAsync();

                _logger.LogInformation("{OrderId} numaralı siparişin durumu 'İade Edildi' olarak güncellendi.", siparis.Id);
            }
            else
            {
                _logger.LogWarning("{OrderId} numaralı sipariş bulunamadı, iade durumu güncellenemedi.", eventMessage.OrderId);
            }
        }
    }
}
