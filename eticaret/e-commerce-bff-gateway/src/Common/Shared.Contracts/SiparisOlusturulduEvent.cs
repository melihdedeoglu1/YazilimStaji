namespace Shared.Contracts
{
    public class SiparisOlusturulduEvent
    {
        public int SiparisId { get; set; }
        public int KullaniciId { get; set; }
        public string KullaniciEmail { get; set; } = string.Empty;  
        public double ToplamTutar { get; set; }
        public List<SiparisKalemiDto> SiparisKalemleri { get; set; } = new();
    }

    
    public class SiparisKalemiDto
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; } = string.Empty;
        public double Fiyat { get; set; }
        public int Adet { get; set; }
        
    }
}
