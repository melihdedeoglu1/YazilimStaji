using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class SiparisSagaBaslatildiEvent
    {      
        public Guid CorrelationId { get; set; }
       
        public int KullaniciId { get; set; }
        public string KullaniciEmail { get; set; } = string.Empty;
        public string KullaniciRol { get; set; } = string.Empty;
        public string KullaniciAdi { get; set; } = string.Empty;
    
        public DateTime TalepTarihi { get; set; }        
        public DateTime UserDate { get; set; }     
        
        public double ToplamTutar { get; set; }
        public List<SiparisKalemiSagaDto> SiparisKalemleri { get; set; } = new();
    }

    public class SiparisKalemiSagaDto
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; } = string.Empty;
        public double Fiyat { get; set; }
        public int Adet { get; set; }

    }




    public class SiparisIptalTalebiEvent
    {
        public Guid CorrelationId { get; set; }
    }



    public class SiparisTamamlandiEvent
    {
        public Guid CorrelationId { get; set; }
        public int KullaniciId { get; set; }
        public string KullaniciEmail { get; set; } = string.Empty;
        public string KullaniciRol { get; set; } = string.Empty;
        public string KullaniciAdi { get; set; } = string.Empty;
        public DateTime TalepTarihi { get; set; }
        public DateTime UserDate { get; set; }
        public double ToplamTutar { get; set; }
        public List<SiparisKalemiSagaDto> SiparisKalemleri { get; set; } = new();
    }

    public class SiparisIptalEdildiEvent
    {
        public Guid CorrelationId { get; set; }
        public string HataMesaji { get; set; } = string.Empty;
    }

    




    public class OdemeYapCommand
    {
        public Guid CorrelationId { get; set; }
        public int KullaniciId { get; set; }
        public double Tutar { get; set; }
    }

    public class OdemeBasariliEvent
    {
        public Guid CorrelationId { get; set; }
    }

    public class OdemeBasarisizEvent
    {
        public Guid CorrelationId { get; set; }
        public string HataMesaji { get; set; } = string.Empty;
    }

    public class OdemeIadeEtCommand 
    {
        public Guid CorrelationId { get; set; }
    }





    

    public class StokAyirCommand
    {
        public Guid CorrelationId { get; set; }
        public List<SiparisKalemiSagaDto> SiparisKalemleri { get; set; } = new();
    }

    public class StokAyrildiEvent
    {
        public Guid CorrelationId { get; set; }
    }

    public class StokYetersizEvent
    {
        public Guid CorrelationId { get; set; }
        public List<int> YetersizUrunIdleri { get; set; } = new();
    }

    public class StokSerbestBirakCommand
    {
        public Guid CorrelationId { get; set; }
        public List<SiparisKalemiSagaDto> SiparisKalemleri { get; set; } = new();
    }




    public class KargoHazirlaCommand
    {
        public Guid CorrelationId { get; set; }      
        public int KullaniciId { get; set; }
        public int SiparisId { get; set; }
        public List<SiparisKalemiSagaDto> SiparisKalemleri { get; set; } = new();
    }


    public class KargoHazirlandiEvent
    {
        public Guid CorrelationId { get; set; }
    }

   
    public class KargoHazirlanamadiEvent
    {
        public Guid CorrelationId { get; set; }
        public string HataMesaji { get; set; } = string.Empty;
    }







}
