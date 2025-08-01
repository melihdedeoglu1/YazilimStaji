using MassTransit;
using Microsoft.EntityFrameworkCore;
using Odeme.API.Data;
using Shared.Contracts;

namespace Odeme.API.Consumers
{
    public class OdemeIadeEtCommandConsumer : IConsumer<OdemeIadeEtCommand>
    {
        private readonly OdemeContext _context;
        private readonly ILogger<OdemeIadeEtCommandConsumer> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public OdemeIadeEtCommandConsumer(OdemeContext context, ILogger<OdemeIadeEtCommandConsumer> logger, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Consume(ConsumeContext<OdemeIadeEtCommand> context)
        {
            var command = context.Message;
            _logger.LogInformation("{CorrelationId} ID'li siparişin iptali nedeniyle ödeme iade ediliyor.", command.CorrelationId);

           
            var odemeKaydi = await _context.OdemeKayitlari.FirstOrDefaultAsync(o => o.CorrelationId == command.CorrelationId && o.Durum == "Basarili");

            if (odemeKaydi != null)
            {
                
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var response = await client.PostAsync($"http://kullanici-servisi:8080/api/user/{odemeKaydi.KullaniciId}/bakiye-iade-et",
                        new StringContent(odemeKaydi.Tutar.ToString()));

                    if (response.IsSuccessStatusCode)
                    {
                        
                        odemeKaydi.Durum = "IadeEdildi";
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("{CorrelationId} ID'li sipariş için ödeme başarıyla iade edildi.", command.CorrelationId);
                    }
                    else
                    {
                        _logger.LogError("{CorrelationId} ID'li sipariş için bakiye iade edilemedi!", command.CorrelationId);
                        
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{CorrelationId} ID'li sipariş için bakiye iadesi sırasında Kullanici.API'ye ulaşılamadı.", command.CorrelationId);
                }
            }
            else
            {
                _logger.LogWarning("{CorrelationId} ID'li iade edilecek başarılı bir ödeme kaydı bulunamadı.", command.CorrelationId);
            }
        }
    }
}