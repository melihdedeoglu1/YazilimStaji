using Iade.API.Data;
using Iade.API.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;


namespace Iade.API.Consumers
{
    public class SiparisIadeTalebiConsumer : IConsumer<SiparisIadeTalebiEvent>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<SiparisIadeTalebiConsumer> _logger;
        private readonly IadeContext _iadeContext;

        public SiparisIadeTalebiConsumer(IHttpClientFactory httpClientFactory, IPublishEndpoint publishEndpoint, ILogger<SiparisIadeTalebiConsumer> logger, IadeContext iadecontext)
        {
            _httpClientFactory = httpClientFactory;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _iadeContext = iadecontext;
        }

        public async Task Consume(ConsumeContext<SiparisIadeTalebiEvent> context)
        {
            var iadeEvent = context.Message;

            var refundLog = new RefundLog
            {
                UserId = iadeEvent.UserId,
                OrderId = iadeEvent.OrderId,
                RefundPrice = iadeEvent.RefundPrice,
                Products = new List<RefundProduct>()
            };

            foreach (var productInEvent in iadeEvent.Products)
            {
                refundLog.Products.Add(new RefundProduct
                {
                    ProductId = productInEvent.ProductId,
                    ProductName = productInEvent.ProductName,
                    ProductQuantity = productInEvent.ProductQuantity
                });
            }

            await _iadeContext.RefundLogs.AddAsync(refundLog);
            
            await _iadeContext.SaveChangesAsync();



            var onayEventi = new IadeOnaylandiEvent
            {
                OrderId = iadeEvent.OrderId,
                UserId = iadeEvent.UserId,
                Products = iadeEvent.Products.Select(p => new IadeEdilenUrun
                {
                    ProductId = p.ProductId,
                    Quantity = p.ProductQuantity
                }).ToList()
            };

           
            await _publishEndpoint.Publish(onayEventi);


        }

    }
}
