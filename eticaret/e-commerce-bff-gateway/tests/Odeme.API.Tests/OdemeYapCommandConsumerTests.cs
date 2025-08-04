using Xunit;
using Moq;
using Moq.Protected;
using MassTransit;
using Microsoft.Extensions.Logging;
using Odeme.API.Consumers;
using Odeme.API.Data;
using Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Linq;

namespace Odeme.API.Tests
{
    public class OdemeYapCommandConsumerTests
    {
        private readonly OdemeContext _context;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly OdemeYapCommandConsumer _consumer;

        public OdemeYapCommandConsumerTests()
        {
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            var loggerMock = new Mock<ILogger<OdemeYapCommandConsumer>>();

            var options = new DbContextOptionsBuilder<OdemeContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new OdemeContext(options);

            
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://kullanici-servisi:8080/") 
            };
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _consumer = new OdemeYapCommandConsumer(
                _httpClientFactoryMock.Object,
                loggerMock.Object,
                _context,
                _publishEndpointMock.Object);
        }

        [Fact]
        public async Task Consume_KullaniciApiBasariliDondugunde_OdemeBasariliEventYayinlamali()
        {
            // Arrange hazırlık işlemleri

            var command = new OdemeYapCommand { KullaniciId = 1, Tutar = 100, CorrelationId = Guid.NewGuid() };

            
            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
               });

            var consumeContextMock = new Mock<ConsumeContext<OdemeYapCommand>>();
            consumeContextMock.Setup(x => x.Message).Returns(command);


            // Act fonksiyonun çağrılması işlemleri

            await _consumer.Consume(consumeContextMock.Object);


            // Assert karşılaştırma işlemleri
            
            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<OdemeBasariliEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<OdemeBasarisizEvent>(), It.IsAny<CancellationToken>()), Times.Never);

            
            var kayit = await _context.OdemeKayitlari.SingleOrDefaultAsync();
            Assert.NotNull(kayit);
            Assert.Equal("Basarili", kayit.Durum);
        }

        [Fact]
        public async Task Consume_KullaniciApiBasarisizDondugunde_OdemeBasarisizEventYayinlamali()
        {
            // Arrange hazırlık işlemleri

            var command = new OdemeYapCommand { KullaniciId = 1, Tutar = 100, CorrelationId = Guid.NewGuid() };

           
            _handlerMock
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                 "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>()
              )
              .ReturnsAsync(new HttpResponseMessage
              {
                  StatusCode = HttpStatusCode.BadRequest, // Yetersiz bakiye durumu gibi
              });

            var consumeContextMock = new Mock<ConsumeContext<OdemeYapCommand>>();
            consumeContextMock.Setup(x => x.Message).Returns(command);


            // Act fonksiyonun çağrılması işlemleri

            await _consumer.Consume(consumeContextMock.Object);


            // Assert karşılaştırma işlemleri
           
            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<OdemeBasarisizEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<OdemeBasariliEvent>(), It.IsAny<CancellationToken>()), Times.Never);

           
            var kayit = await _context.OdemeKayitlari.SingleOrDefaultAsync();
            Assert.NotNull(kayit);
            Assert.Equal("Basarisiz", kayit.Durum);
        }
    }
}
