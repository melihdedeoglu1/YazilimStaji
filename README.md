# YazilimStaji
Yazılım yaz stajımda öğrendiklerim ve kendimi geliştirmek için yaptığım çalışmalar.

<details>
  <summary><strong>1. Saga pattern mikroservis mimarisinde hangi sorunları çözmeye çalışır?</strong></summary>
  <br>
  
</details>

---

<details>
  <summary><strong>2. Saga pattern'deki choreography ve orchestration yaklaşımları arasındaki temel fark nedir?</strong></summary>
  <br>
  
  **Saga pattern'deki Choreography yaklaşımı:** Yerel işlemlerin her biri, diğer hizmetlerdeki yerel işlemlerini tetikleyen domain(alan) olayı yayınlar. Kendi tamamlandıktan sonra diğer işlemin de başlamasını sağlar. Olay yayınlandıktan sonra diğer servisler dinler ve tetiklenirse işlem yapar. 

  **Saga pattern'deki Orchestration yaklaşımı:** Bir saga koordinatörü hangi servisin yerel işlemlerinin yürütüleceğini söyler. Komut tabanlı olup servislere komut gönderir ve servislerdeki komut işleyiciler bu komutu alarak işlemlerini yaparlar.

  **Karşılaştırma:** Choreography yaklaşımı dağıtık kontrollü olup event yayınlama ile iletişim sağlar. Orchestration yaklaşımı ise merkezi kontrollü olup komut gönderme ile iletişim sağlar.
  
</details>

---

<details>
  <summary><strong>3. Orchestration Saga pattern avantajları ve dezavantajları nelerdir?</strong></summary>
  <br>

  **Orchestration Saga pattern avantajları:** Bir servise komut gönderdikten sonra doğru bir sonuç geldiğinde diğer hizmete yeni komutu göndererek düzeni sağlar. Yanlış bir sonuç geldiğinde de eski servislerdeki geri alma komutlarını çalıştırarak yanlışlığı engeller.

  **Orchestration Saga pattern dezavantajları:** Çok adım gerektiğinde karmaşıklık meydana gelebilir. Her adımın sadece komutu işlemesi hariç telafi edici işlemleri de olduğu için tüm senaryoları düşünmek zordur ve çaba gerektirir. 
 
</details>
