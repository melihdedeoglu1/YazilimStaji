using System.ComponentModel.DataAnnotations;

namespace Kargo.API.Models
{
    public class KargoGonderi
    {        
        public int Id { get; set; }
        [Required]
        public int SiparisId { get; set; }
        public int KullaniciId { get; set; }            
        public string Durum { get; set; } = string.Empty;
        public DateTime OlusturmaTarihi { get; set; }

        public DateTime? GonderimTarihi { get; set; }
        public DateTime? TahminiTeslimTarihi { get; set; }
    }
}
