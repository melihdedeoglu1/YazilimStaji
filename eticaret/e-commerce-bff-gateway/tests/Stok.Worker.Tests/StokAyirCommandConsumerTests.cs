using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Contracts;
using Stok.Worker.Consumers;
using Stok.Worker.Data;
using Stok.Worker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xunit;

namespace Stok.Worker.Tests
{
    public class StokAyirCommandConsumerTests
    {
        
        private readonly ProductContext _context;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly StokAyirCommandConsumer _consumer;

        public StokAyirCommandConsumerTests()
        {
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            var loggerMock = new Mock<ILogger<StokAyirCommandConsumer>>();

            
            
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductContext(options);

            _consumer = new StokAyirCommandConsumer(
                _context, 
                loggerMock.Object,
                _publishEndpointMock.Object);
        }

        [Fact]
        public async Task Consume_StokYeterliOldugunda_StokAyrildiEventiYayinlamali()
        {
            
            _context.Products.Add(new Product { Id = 1, StockQuantity = 20 });
            _context.Products.Add(new Product { Id = 2, StockQuantity = 15 });
            await _context.SaveChangesAsync();

            
            var command = new StokAyirCommand
            {
                CorrelationId = Guid.NewGuid(),
                SiparisKalemleri = new List<SiparisKalemiSagaDto>
                {
                    new SiparisKalemiSagaDto { UrunId = 1, Adet = 5 },
                    new SiparisKalemiSagaDto { UrunId = 2, Adet = 10 }
                }
            };

            var consumeContextMock = new Mock<ConsumeContext<StokAyirCommand>>();
            consumeContextMock.Setup(x => x.Message).Returns(command);

            
            await _consumer.Consume(consumeContextMock.Object);

            
            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<StokAyrildiEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<StokYetersizEvent>(), It.IsAny<CancellationToken>()), Times.Never);

           
            var urun1 = await _context.Products.FindAsync(1);
            Assert.Equal(15, urun1.StockQuantity); 
        }

        [Fact]
        public async Task Consume_StokYetersizOldugunda_StokYetersizEventiYayinlamali()
        {
           
            _context.Products.Add(new Product { Id = 1, StockQuantity = 20 });
            _context.Products.Add(new Product { Id = 2, StockQuantity = 5 }); 
            await _context.SaveChangesAsync();

           
            var command = new StokAyirCommand
            {
                CorrelationId = Guid.NewGuid(),
                SiparisKalemleri = new List<SiparisKalemiSagaDto>
                {
                    new SiparisKalemiSagaDto { UrunId = 1, Adet = 5 },
                    new SiparisKalemiSagaDto { UrunId = 2, Adet = 10 } 
                }
            };

            var consumeContextMock = new Mock<ConsumeContext<StokAyirCommand>>();
            consumeContextMock.Setup(x => x.Message).Returns(command);

            
            await _consumer.Consume(consumeContextMock.Object);

            
            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<StokYetersizEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<StokAyrildiEvent>(), It.IsAny<CancellationToken>()), Times.Never);

            
            var urun2 = await _context.Products.FindAsync(2);
            Assert.Equal(5, urun2.StockQuantity); 
        }
    }
}