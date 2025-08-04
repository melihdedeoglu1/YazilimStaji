using Xunit;
using Moq;
using MassTransit;
using Microsoft.Extensions.Logging;
using Stok.Worker.Consumers;
using Stok.Worker.Data;
using Stok.Worker.Models;
using Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace Stok.Worker.Tests
{
    public class StokSerbestBirakCommandConsumerTests
    {
        private readonly ProductContext _context;
        private readonly StokSerbestBirakCommandConsumer _consumer;

        public StokSerbestBirakCommandConsumerTests()
        {
            var loggerMock = new Mock<ILogger<StokSerbestBirakCommandConsumer>>();

            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductContext(options);

            _consumer = new StokSerbestBirakCommandConsumer(
                _context,
                loggerMock.Object);
        }

        [Fact]
        public async Task Consume_MevcutUrunlerIcin_StokMiktariniDogruSekildeArtirmali()
        {          
            
            _context.Products.Add(new Product { Id = 1, StockQuantity = 10 });
            _context.Products.Add(new Product { Id = 2, StockQuantity = 5 });
            await _context.SaveChangesAsync();

            
            var command = new StokSerbestBirakCommand
            {
                CorrelationId = Guid.NewGuid(),
                SiparisKalemleri = new List<SiparisKalemiSagaDto>
                {
                    new SiparisKalemiSagaDto { UrunId = 1, Adet = 5 }, 
                    new SiparisKalemiSagaDto { UrunId = 2, Adet = 3 }  
                }
            };

            var consumeContextMock = new Mock<ConsumeContext<StokSerbestBirakCommand>>();
            consumeContextMock.Setup(x => x.Message).Returns(command);
         
            await _consumer.Consume(consumeContextMock.Object);
           
            var urun1 = await _context.Products.FindAsync(1);
            var urun2 = await _context.Products.FindAsync(2);

            Assert.Equal(15, urun1.StockQuantity);
            Assert.Equal(8, urun2.StockQuantity);  
        }

        [Fact]
        public async Task Consume_MevcutOlmayanUrunIdsiGeldiginde_HataVermedenTamamlanmali()
        {
            
            _context.Products.Add(new Product { Id = 1, StockQuantity = 10 });
            await _context.SaveChangesAsync();

            
            var command = new StokSerbestBirakCommand
            {
                CorrelationId = Guid.NewGuid(),
                SiparisKalemleri = new List<SiparisKalemiSagaDto>
                {
                    new SiparisKalemiSagaDto { UrunId = 1, Adet = 5 },       
                    new SiparisKalemiSagaDto { UrunId = 99, Adet = 10 }     
                }
            };

            var consumeContextMock = new Mock<ConsumeContext<StokSerbestBirakCommand>>();
            consumeContextMock.Setup(x => x.Message).Returns(command);

            
            await _consumer.Consume(consumeContextMock.Object);

            
            var urun1 = await _context.Products.FindAsync(1);
            Assert.Equal(15, urun1.StockQuantity);
        }
    }
}
