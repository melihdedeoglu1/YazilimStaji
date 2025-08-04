using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Odeme.API.Consumers;
using Odeme.API.Data;
using Odeme.API.Models;
using Shared.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Odeme.API.Tests
{
    public class OdemeIadeEtCommandConsumerTests
    {
        private readonly OdemeContext _context;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly OdemeIadeEtCommandConsumer _consumer;

        public OdemeIadeEtCommandConsumerTests()
        {
            var loggerMock = new Mock<ILogger<OdemeIadeEtCommandConsumer>>();

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

            _consumer = new OdemeIadeEtCommandConsumer(
                _context,
                loggerMock.Object,
                _httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task Consume_KayitBulunursaVeApiBasariliOlursa_DurumIadeEdildiOlarakGuncellenmeli()
        {
            // Arrange hazırlık işlemleri

            var correlationId = Guid.NewGuid();
            var odemeKaydi = new OdemeKaydi { Id = 1, CorrelationId = correlationId, KullaniciId = 123, Tutar = 150, Durum = "Basarili" };
            _context.OdemeKayitlari.Add(odemeKaydi);
            await _context.SaveChangesAsync();

            var command = new OdemeIadeEtCommand { CorrelationId = correlationId };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var consumeContextMock = new Mock<ConsumeContext<OdemeIadeEtCommand>>();
            consumeContextMock.Setup(x => x.Message).Returns(command);


            // Act fonksiyonun çağrılması işlemleri

            await _consumer.Consume(consumeContextMock.Object);
            

            // Assert karşılaştırma işlemleri

            var guncellenenKayit = await _context.OdemeKayitlari.FindAsync(1);
            Assert.Equal("IadeEdildi", guncellenenKayit.Durum);
        }

        [Fact]
        public async Task Consume_KayitBulunursaVeApiBasarisizOlursa_DurumDegismemeli()
        {
            // Arrange hazırlık işlemleri

            var correlationId = Guid.NewGuid();
            var odemeKaydi = new OdemeKaydi { Id = 1, CorrelationId = correlationId, KullaniciId = 123, Tutar = 150, Durum = "Basarili" };
            _context.OdemeKayitlari.Add(odemeKaydi);
            await _context.SaveChangesAsync();

            var command = new OdemeIadeEtCommand { CorrelationId = correlationId };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError });

            var consumeContextMock = new Mock<ConsumeContext<OdemeIadeEtCommand>>();
            consumeContextMock.Setup(x => x.Message).Returns(command);


            // Act fonksiyonun çağrılması işlemleri

            await _consumer.Consume(consumeContextMock.Object);


            // Assert karşılaştırma işlemleri

            var guncellenenKayit = await _context.OdemeKayitlari.FindAsync(1);
            Assert.Equal("Basarili", guncellenenKayit.Durum); 
        }

        [Fact]
        public async Task Consume_IadeEdilecekKayitBulunamazsa_IslemYapilmamali()
        {
            // Arrange hazırlık işlemleri

            var correlationId = Guid.NewGuid();
            
            _context.OdemeKayitlari.Add(new OdemeKaydi { Id = 1, CorrelationId = correlationId, Durum = "Basarisiz" });
            await _context.SaveChangesAsync();

            var command = new OdemeIadeEtCommand { CorrelationId = correlationId };
            var consumeContextMock = new Mock<ConsumeContext<OdemeIadeEtCommand>>();
            consumeContextMock.Setup(x => x.Message).Returns(command);


            // Act fonksiyonun çağrılması işlemleri

            await _consumer.Consume(consumeContextMock.Object);


            // Assert karşılaştırma işlemleri
            
            _handlerMock.Protected().Verify(
               "SendAsync",
               Times.Never(), // Hiç çağrılmamalı
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>()
            );

           
            var kayit = await _context.OdemeKayitlari.FindAsync(1);
            Assert.Equal("Basarisiz", kayit.Durum);
        }
    }
}
