using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odeme.API.Models
{
    public class OdemeKaydi
    {
       
        public int Id { get; set; }
        public Guid CorrelationId { get; set; }
        public int KullaniciId { get; set; }       
        public double Tutar { get; set; }
        public DateTime OdemeTarihi { get; set; }
        public string Durum { get; set; } = string.Empty;       
        public string? IslemReferansNo { get; set; }
    }
}
