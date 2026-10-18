# Lost Heaven Cafe

Yalova Armutlu'daki bir çay bahçesi için hazırlanmış QR menü ve masa takip uygulamasıdır.

Amaç: Mekânda masa numaraları bulunmuyor, adisyonlar kâğıt üzerinde elle tutuluyor ve müşteriler menüye kolay ulaşamıyordu. Uygulama bu sorunları çözmek için hazırlanmıştır.

Özellikler:

QR menü: Müşteriler QR kodu okutarak giriş yapmadan menüyü görüntüler.
Menü kategorilere ayrılmıştır; her üründe ad, fiyat ve fotoğraf bulunur.

Masa takibi: 30 masa tek ekranda listelenir. Boş masalar yeşil, dolu masalar kırmızı gösterilir.
Masaya tıklanınca ürün eklenir veya çıkarılır, hesap toplamı otomatik hesaplanır.
Hesap kapatıldığında masa boşa alınır ve adisyon kayıt altında tutulur.
Açık hesaplar, program kapatılsa bile silinmez.

Kasa: Günlük toplam ve genel toplam ayrı gösterilir.
"Günü Bitir" işlemi yalnızca günlük toplamı sıfırlar, genel toplam korunur.
Güncel tarih ve saat ekranda canlı olarak görünür.

Hatırlatmalar: Tarihli notlar eklenir (örneğin toptancının geliş günü).
Tamamlanan hatırlatmalar işaretlenir, tarihi geçenler kırmızı gösterilir.
Yaklaşan hatırlatmalar kasa ekranında da görünür.

Menü yönetimi: Yeni ürün eklenir; ürün resmi, web adresi veya bilgisayardan dosya olarak yüklenebilir.
Ürün geçici olarak menüden kaldırılabilir ("Pasif Yap") veya tamamen silinebilir. Daha önce satılmış ürünler, geçmiş kayıtlar bozulmasın diye silinemez.

Diğer: Menü adresi için QR kod oluşturma ekranı bulunur.
Panele yalnızca kayıtlı ve giriş yapmış kullanıcılar erişir.
Tüm veriler veritabanında kalıcı olarak saklanır.

Kullanılan teknolojiler
Sunucu tarafı: C#, ASP.NET Core MVC (.NET 8)

Veritabanı: SQLite, Entity Framework Core

Giriş ve kayıt: ASP.NET Core Identity

Arayüz: HTML, CSS, JavaScript

Durum: Uygulamanın önümüzdeki yaz çay bahçesinde kullanılmaya başlanması planlanmaktadır.

Not: Proje geliştirilirken yapay zekâ araçlarından yararlanılmıştır.
