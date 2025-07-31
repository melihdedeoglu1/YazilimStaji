using MassTransit;
using Shared.Contracts;
using System; 

namespace Siparis.API.Sagas
{
    public class SiparisSagaStateMachine : MassTransitStateMachine<SiparisSagaState>
    {
        public State OdemeBekleniyor { get; private set; }
        public State StokBekleniyor { get; private set; }

        public Event<SiparisSagaBaslatildiEvent> SagaBaslatildiEvent { get; private set; }
        public Event<OdemeBasariliEvent> OdemeBasariliEvent { get; private set; }
        public Event<OdemeBasarisizEvent> OdemeBasarisizEvent { get; private set; }
        public Event<StokAyrildiEvent> StokAyrildiEvent { get; private set; }
        public Event<StokYetersizEvent> StokYetersizEvent { get; private set; }

        public SiparisSagaStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Initially(
                When(SagaBaslatildiEvent)
                    .Then(context =>
                    {
                        context.Saga.KullaniciId = context.Message.KullaniciId;
                        context.Saga.ToplamTutar = context.Message.ToplamTutar;
                        context.Saga.OlusturmaTarihi = context.Message.TalepTarihi;
                        
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
                    
                    .Send(new Uri("queue:odeme-yap-kuyrugu"), context =>
                        new OdemeYapCommand
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
                    .Finalize()
            );

            During(OdemeBekleniyor,
                When(OdemeBasariliEvent)
                   
                    .Publish(context => new SiparisTamamlandiEvent
                    {
                        CorrelationId = context.Saga.CorrelationId
                    })
                    .Finalize(),

                When(OdemeBasarisizEvent)
                    
                    .Send(new Uri("queue:stok-serbest-birak-kuyrugu"), context =>
                        new StokSerbestBirakCommand
                        {
                            CorrelationId = context.Saga.CorrelationId,
                            SiparisKalemleri = context.Saga.SiparisKalemleri
                        })
                    .Publish(context => new SiparisIptalEdildiEvent
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        HataMesaji = context.Message.HataMesaji
                    })
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }
    }
}