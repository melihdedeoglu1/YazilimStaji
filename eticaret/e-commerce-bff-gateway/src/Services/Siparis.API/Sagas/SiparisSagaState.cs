using MassTransit;
using Shared.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Siparis.API.Sagas
{
    public class SiparisSagaState : SagaStateMachineInstance
    {
        [Key]
        public Guid CorrelationId { get; set; }       
        public string CurrentState { get; set; } = string.Empty;      
        public int KullaniciId { get; set; }
        public int SiparisId { get; set; } 
        public double ToplamTutar { get; set; }
        public DateTime OlusturmaTarihi { get; set; }

        public List<SiparisKalemiSagaDto> SiparisKalemleri { get; set; } = new();


        public string KullaniciEmail { get; set; } = string.Empty;
        public string KullaniciRol { get; set; } = string.Empty;
        public string KullaniciAdi { get; set; } = string.Empty;
        public DateTime UserDate { get; set; }

    }
}
