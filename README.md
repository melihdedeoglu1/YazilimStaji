# YazilimStaji
Yazılım yaz stajımda öğrendiklerim ve kendimi geliştirmek için yaptığım çalışmalar.

# Saga Pattern

<details>
  <summary><strong>1. Saga pattern mikroservis mimarisinde hangi sorunları çözmeye çalışır?</strong></summary>
  <br>

  **Çözdüğü sorun:** Mikroservis mimarisinde her servisin kendi veri tabanı olduğu için veri tutarlılıklarını sağlamak zordur. Saga Pattern sayesinde isteklerin başarılı oldukça devam etmesi ve hata durumlarında geri alma aksiyonu ile veri tutarlılıkları sağlamak kolaylaşır.
  
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

---

<details>
  <summary><strong>4.1. Bu süreci yönetmek için bir Saga pattern tasarlayın ve basit bir durum makinesi (state machine) diyagramı çizin. Sipariş Verildi aşamasından Sipariş Tamamlandı aşamasına kadar olan her bir durumu çizin ve her bir başarısızlık durumunda geri alma adımlarını gösterin.</strong></summary>
  <br>

  ![StateMachine](statemachine.png)
  
 
</details>

---

<details>
  <summary><strong>4.2. Her bir durumda, ilgili hizmetin başarılı ya da başarısız olması durumunda nasıl bir geçiş yapılacağını açıklayın.</strong></summary>
  <br>

  ![Tablo](tablo.png)
  
 
</details>


# Xunit ve Moq

<details>
  <summary><strong>2. Xunit ve Moq Temel Kavramları.</strong></summary>
  <br>
  
  **Xunit:** Unit Test, bir yazılımın en küçük test edilebilir bölümlerinin(sınıflar,metodlar vs.), tek tek ve bağımsız olarak doğru çalışabilirliğinin incelendiği bir yazılım geliştirme sürecidir. Xunit ise popüler Unit Test Frameworklerinden biridir. 

  **Moq:** C# dilinde ve birim testlerinde yaygın olarak kullanılan bir mocking(taklit nesne oluşturma) kütüphanesidir. 
  
</details>
