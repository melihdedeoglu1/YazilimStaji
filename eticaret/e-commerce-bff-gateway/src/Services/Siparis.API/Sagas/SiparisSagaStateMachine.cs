using MassTransit;
using Shared.Contracts;
using Siparis.API.Data;
using Siparis.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Siparis.API.Sagas
{
    public class SiparisSagaStateMachine : MassTransitStateMachine<SiparisSagaState>
    {
        private readonly SiparisContext _siparisContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public State StokBekleniyor { get; private set; }
        public State OdemeBekleniyor { get; private set; }
        public State KargoBekleniyor { get; private set; }

        public Event<SiparisSagaBaslatildiEvent> SagaBaslatildiEvent { get; private set; }
        public Event<StokAyrildiEvent> StokAyrildiEvent { get; private set; }
        public Event<StokYetersizEvent> StokYetersizEvent { get; private set; }
        public Event<OdemeBasariliEvent> OdemeBasariliEvent { get; private set; }
        public Event<OdemeBasarisizEvent> OdemeBasarisizEvent { get; private set; }
        public Event<KargoHazirlandiEvent> KargoHazirlandiEvent { get; private set; }
        public Event<KargoHazirlanamadiEvent> KargoHazirlanamadiEvent { get; private set; }
        public Event<SiparisIptalTalebiEvent> IptalTalebiEvent { get; private set; }

        public SiparisSagaStateMachine(SiparisContext siparisContext, IPublishEndpoint publishEndpoint)
        {
            _siparisContext = siparisContext;
            _publishEndpoint = publishEndpoint;

            InstanceState(x => x.CurrentState);

            Event(() => SagaBaslatildiEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => StokAyrildiEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => StokYetersizEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => OdemeBasariliEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => OdemeBasarisizEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => KargoHazirlandiEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => KargoHazirlanamadiEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => IptalTalebiEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(SagaBaslatildiEvent)
                    .Then(context =>
                    {
                        context.Saga.KullaniciId = context.Message.KullaniciId;
                        context.Saga.KullaniciEmail = context.Message.KullaniciEmail;
                        context.Saga.KullaniciRol = context.Message.KullaniciRol;
                        context.Saga.KullaniciAdi = context.Message.KullaniciAdi;
                        context.Saga.ToplamTutar = context.Message.ToplamTutar;
                        context.Saga.OlusturmaTarihi = context.Message.TalepTarihi;
                        context.Saga.UserDate = context.Message.UserDate;
                        context.Saga.SiparisKalemleri = context.Message.SiparisKalemleri;
                    })
                    .Send(new Uri("queue:stok-ayir-kuyrugu"), context =>
                        new StokAyirCommand
                        {
                            CorrelationId = context.Saga.CorrelationId,
                            SiparisKalemleri = context.Message.SiparisKalemleri
                        })
                    .TransitionTo(StokBekleniyor)
            );

            During(StokBekleniyor,
                When(StokAyrildiEvent)
                    .Send(new Uri("queue:odeme-yap-kuyrugu"), context => new OdemeYapCommand
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        KullaniciId = context.Saga.KullaniciId,
                        Tutar = context.Saga.ToplamTutar
                    })
                    .TransitionTo(OdemeBekleniyor),

                When(StokYetersizEvent)
                    .Publish(context => new SiparisIptalEdildiEvent
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        HataMesaji = "Stok yetersiz."
                    })
                    .Finalize(),

                When(IptalTalebiEvent)
                    .Send(new Uri("queue:stok-serbest-birak-kuyrugu"), context => new StokSerbestBirakCommand
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        SiparisKalemleri = context.Saga.SiparisKalemleri
                    })
                    .Publish(context => new SiparisIptalEdildiEvent
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        HataMesaji = "Kullanıcı tarafından iptal edildi."
                    })
                    .Finalize()
            );

            During(OdemeBekleniyor,
                When(OdemeBasariliEvent)
                    .ThenAsync(async context =>
                    {
                        var newOrder = new Order
                        {
                            UserId = context.Saga.KullaniciId,
                            TotalPrice = context.Saga.ToplamTutar,
                            OrderDate = context.Saga.OlusturmaTarihi,
                            Status = "Hazırlanıyor",
                            OrderItems = context.Saga.SiparisKalemleri.Select(item => new OrderItem
                            {
                                ProductId = item.UrunId,
                                ProductName = item.UrunAdi,
                                Price = item.Fiyat,
                                Quantity = item.Adet
                            }).ToList()
                        };
                        await _siparisContext.Orders.AddAsync(newOrder);
                        await _siparisContext.SaveChangesAsync();
                        context.Saga.SiparisId = newOrder.Id;
                    })
                    .Send(new Uri("queue:kargo-hazirla-kuyrugu"), context => new KargoHazirlaCommand
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        SiparisId = context.Saga.SiparisId,
                        KullaniciId = context.Saga.KullaniciId,
                        SiparisKalemleri = context.Saga.SiparisKalemleri
                    })
                    .TransitionTo(KargoBekleniyor),

                When(OdemeBasarisizEvent)
                    .Send(new Uri("queue:stok-serbest-birak-kuyrugu"), context => new StokSerbestBirakCommand
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        SiparisKalemleri = context.Saga.SiparisKalemleri
                    })
                    .Publish(context => new SiparisIptalEdildiEvent
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        HataMesaji = context.Message.HataMesaji
                    })
                    .Finalize(),

                When(IptalTalebiEvent)
                    .Send(new Uri("queue:stok-serbest-birak-kuyrugu"), context => new StokSerbestBirakCommand
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        SiparisKalemleri = context.Saga.SiparisKalemleri
                    })
                    .Publish(context => new SiparisIptalEdildiEvent
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        HataMesaji = "Kullanıcı tarafından iptal edildi."
                    })
                    .Finalize()
            );

            During(KargoBekleniyor,
                When(KargoHazirlandiEvent)
                    .ThenAsync(async context =>
                    {
                        var eventMessage = new SiparisOlusturulduEvent
                        {
                            SiparisId = context.Saga.SiparisId,
                            KullaniciId = context.Saga.KullaniciId,
                            KullaniciEmail = context.Saga.KullaniciEmail,
                            ToplamTutar = context.Saga.ToplamTutar,
                            SiparisTarihi = context.Saga.OlusturmaTarihi,
                            Durum = "Tamamlandı",
                            KullaniciRol = context.Saga.KullaniciRol,
                            KullaniciAdi = context.Saga.KullaniciAdi,
                            UserDate = context.Saga.UserDate,
                            SiparisKalemleri = context.Saga.SiparisKalemleri.Select(item => new SiparisKalemiDto
                            {
                                UrunId = item.UrunId,
                                UrunAdi = item.UrunAdi,
                                Fiyat = item.Fiyat,
                                Adet = item.Adet
                            }).ToList()
                        };
                        await _publishEndpoint.Publish(eventMessage);
                    })
                    .Finalize(),

                When(KargoHazirlanamadiEvent)
                    .Send(new Uri("queue:odeme-iade-et-kuyrugu"), context => new OdemeIadeEtCommand { CorrelationId = context.Saga.CorrelationId })
                    .Send(new Uri("queue:stok-serbest-birak-kuyrugu"), context => new StokSerbestBirakCommand { SiparisKalemleri = context.Saga.SiparisKalemleri, CorrelationId = context.Saga.CorrelationId })
                    .Publish(context => new SiparisIptalEdildiEvent { HataMesaji = context.Message.HataMesaji, CorrelationId = context.Saga.CorrelationId })
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }
    }
}