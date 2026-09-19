# Lost Heaven Cafe — QR Menü & Masa Takip

Dayımın Yalova Armutlu'daki kafesi için yaptığım bir proje. Amaç basit: fiziki
adisyon ve masa no derdini bitirip, QR ile açılan bir menü + masaları tek
panelden takip edebileceğim bir sistem kurmak.

**İlk kez yapay zekayı aktif kullandığım projem.**
**QR kısmı harici resmi olarak uygulandı, hayata geçirildi.**

## Nasıl Çalıştırılır

1. `LostHeavenCafe.csproj`'a çift tıkla, Visual Studio açsın.
2. `F5` ile çalıştır. İlk açılışta veritabanı (`lostheaven.db`), 30 masa ve
   örnek bir menü otomatik oluşuyor.
3. Ana sayfadan "Kayıt Ol" ile kendine bir hesap aç, sonra "Giriş Yap".

## Neresi Ne İşe Yarıyor

- **Ana sayfa (`/`)**: Giriş/Kayıt paneli. Buraya QR koymuyoruz.
- **`/Menu`**: Müşterinin QR okutunca göreceği sayfa. QR kodun bu adrese
  gitmesi lazım, ana sayfaya değil.
- **Masalar**: 1-30 masa, boş/dolu renkli. Bir masaya girip +/- ile sipariş
  giriliyor, "Hesabı Kapat" ile kapatılıyor.
- **Kasa**: Ana Toplam (hiç sıfırlanmaz) ve Günlük Toplam ayrı. Her gün
  kapanışta "Günü Bitir"e basmayı unutma, yoksa günlük toplam bir öncekiyle
  karışık gider.
- **Hatırlatmalar**: Toptancı ne zaman gelecek gibi tarihli notlar.
- **Menü Yönetimi**: Yeni ürün ekleme (resim URL yapıştırabilirsin ya da
  bilgisayarından dosya seçebilirsin), var olan ürünün resmini sonradan
  ekleyip değiştirme, "Pasif Yap" (ürün geçici yok, menüden gizlenir ama
  geçmişi durur) ve "Sil" (ürünü tamamen kaldırır — ama bir ürün daha önce
  satılmışsa geçmiş ciro bozulmasın diye silinmesine izin vermiyor, o zaman
  Pasif Yap kullanman lazım).
- **QR Kod Oluştur**: İstediğin adres için anında QR üretip indirebiliyorsun.
