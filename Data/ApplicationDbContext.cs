using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LostHeavenCafe.Models;

namespace LostHeavenCafe.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Masa> Masalar => Set<Masa>();
    public DbSet<MenuKategori> MenuKategorileri => Set<MenuKategori>();
    public DbSet<MenuUrun> MenuUrunleri => Set<MenuUrun>();
    public DbSet<Adisyon> Adisyonlar => Set<Adisyon>();
    public DbSet<AdisyonKalemi> AdisyonKalemleri => Set<AdisyonKalemi>();
    public DbSet<IsGunu> IsGunleri => Set<IsGunu>();
    public DbSet<Hatirlatma> Hatirlatmalar => Set<Hatirlatma>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Masa>().HasIndex(m => m.MasaNo).IsUnique();

        // Bir ürünü sildiğinde, o ürünün geçmişte satılmış kayıtları (AdisyonKalemi)
        // otomatik silinmesin — geçmiş satış/ciro rakamları bozulmasın diye.
        // Geçmişi olan bir ürün silinemez, sadece pasif yapılabilir (bkz. MenuController.UrunSil).
        builder.Entity<AdisyonKalemi>()
            .HasOne(k => k.Urun)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }

    // Uygulama ilk açıldığında veritabanı boşsa 30 masa + örnek çay bahçesi
    // menüsünü otomatik oluşturur. Menüyü /Menu/Yonet ekranından
    // düzenleyebilirsin, bu sadece başlangıç için.
    //
    // Fotoğraflar: Çay ve Kahvaltı Tabağı için gerçek, telifsiz (Unsplash
    // License) fotoğraf URL'leri koydum — bunlar direkt internetten
    // gösteriliyor, projede dosya olarak durmuyor. Diğer ürünler için
    // wwwroot/images/urunler/ klasörüne kendi fotoğraflarını eklemen
    // yeterli (README'de hangi dosya adının hangi ürüne ait olduğu yazıyor).
    public static void OrnekVeriyiOlustur(ApplicationDbContext db)
    {
        if (!db.Masalar.Any())
        {
            for (int i = 1; i <= 30; i++)
                db.Masalar.Add(new Masa { MasaNo = i, Dolu = false });
            db.SaveChanges();
        }

        if (!db.MenuKategorileri.Any())
        {
            var sicakIcecekler = new MenuKategori { Ad = "Sıcak İçecekler", Sira = 1 };
            var sogukIcecekler = new MenuKategori { Ad = "Soğuk İçecekler", Sira = 2 };
            var kahvaltiTabaklari = new MenuKategori { Ad = "Kahvaltı", Sira = 3 };
            var tostlar = new MenuKategori { Ad = "Tostlar", Sira = 4 };

            db.MenuKategorileri.AddRange(sicakIcecekler, sogukIcecekler, kahvaltiTabaklari, tostlar);
            db.SaveChanges();

            db.MenuUrunleri.AddRange(
                new MenuUrun { KategoriId = sicakIcecekler.Id, Ad = "Çay", Fiyat = 20, Resim = "https://images.unsplash.com/photo-1715017245474-5533582014a3?auto=format&fit=crop&w=400&q=70" },
                new MenuUrun { KategoriId = sicakIcecekler.Id, Ad = "Türk Kahvesi", Fiyat = 60, Resim = "https://images.unsplash.com/photo-1506778020041-0ea35027d019?auto=format&fit=crop&w=400&q=70" },
                new MenuUrun { KategoriId = sicakIcecekler.Id, Ad = "Nescafe", Fiyat = 70, Resim = "https://images.unsplash.com/photo-1703013132195-691ce55017c0?auto=format&fit=crop&w=400&q=70" },

                new MenuUrun { KategoriId = sogukIcecekler.Id, Ad = "Limonata", Fiyat = 65, Resim = "https://images.unsplash.com/photo-1623157980612-da2c2cdb4c0b?auto=format&fit=crop&w=400&q=70" },
                new MenuUrun { KategoriId = sogukIcecekler.Id, Ad = "Ayran", Fiyat = 35, Resim = "https://images.unsplash.com/photo-1596151163116-98a5033814c2?auto=format&fit=crop&w=400&q=70" },
                new MenuUrun { KategoriId = sogukIcecekler.Id, Ad = "Soda", Fiyat = 30, Resim = "https://images.unsplash.com/photo-1567517094298-d0d7bbbfb714?auto=format&fit=crop&w=400&q=70" },

                new MenuUrun { KategoriId = kahvaltiTabaklari.Id, Ad = "Kahvaltı Tabağı", Fiyat = 250, Resim = "https://images.unsplash.com/photo-1768319920501-2d124ccfd8dc?auto=format&fit=crop&w=400&q=70" },
                new MenuUrun { KategoriId = kahvaltiTabaklari.Id, Ad = "Menemen", Fiyat = 150, Resim = "https://images.unsplash.com/photo-1520218576172-c1a2df3fa5fc?auto=format&fit=crop&w=400&q=70" },
                new MenuUrun { KategoriId = kahvaltiTabaklari.Id, Ad = "Omlet", Fiyat = 120, Resim = "https://images.unsplash.com/photo-1628484641099-0e7ae67fbdc1?auto=format&fit=crop&w=400&q=70" },

                new MenuUrun { KategoriId = tostlar.Id, Ad = "Kaşarlı Tost", Fiyat = 100, Resim = "https://images.unsplash.com/photo-1634598604019-7fca68ad3b8e?auto=format&fit=crop&w=400&q=70" },
                new MenuUrun { KategoriId = tostlar.Id, Ad = "Karışık Tost", Fiyat = 130, Resim = "https://images.unsplash.com/photo-1528735602780-2552fd46c7af?auto=format&fit=crop&w=400&q=70" }
            );
            db.SaveChanges();
        }
    }

    // Daha önce boş/dosya-adı olarak kurulmuş bir veritabanın varsa (ilk sürümü
    // çalıştırdıysan), bu metod ürün adına göre eşleşen gerçek fotoğraf
    // URL'lerini geriye dönük olarak günceller. Uygulama her açıldığında
    // çalışır, zaten güncel olan bir üründe bir şey değiştirmez.
    public static void UrunFotograflariniGuncelle(ApplicationDbContext db)
    {
        var fotograflar = new Dictionary<string, string>
        {
            ["Çay"] = "https://images.unsplash.com/photo-1715017245474-5533582014a3?auto=format&fit=crop&w=400&q=70",
            ["Türk Kahvesi"] = "https://images.unsplash.com/photo-1506778020041-0ea35027d019?auto=format&fit=crop&w=400&q=70",
            ["Nescafe"] = "https://images.unsplash.com/photo-1703013132195-691ce55017c0?auto=format&fit=crop&w=400&q=70",
            ["Limonata"] = "https://images.unsplash.com/photo-1623157980612-da2c2cdb4c0b?auto=format&fit=crop&w=400&q=70",
            ["Ayran"] = "https://images.unsplash.com/photo-1596151163116-98a5033814c2?auto=format&fit=crop&w=400&q=70",
            ["Soda"] = "https://images.unsplash.com/photo-1567517094298-d0d7bbbfb714?auto=format&fit=crop&w=400&q=70",
            ["Kahvaltı Tabağı"] = "https://images.unsplash.com/photo-1768319920501-2d124ccfd8dc?auto=format&fit=crop&w=400&q=70",
            ["Menemen"] = "https://images.unsplash.com/photo-1520218576172-c1a2df3fa5fc?auto=format&fit=crop&w=400&q=70",
            ["Omlet"] = "https://images.unsplash.com/photo-1628484641099-0e7ae67fbdc1?auto=format&fit=crop&w=400&q=70",
            ["Kaşarlı Tost"] = "https://images.unsplash.com/photo-1634598604019-7fca68ad3b8e?auto=format&fit=crop&w=400&q=70",
            ["Karışık Tost"] = "https://images.unsplash.com/photo-1528735602780-2552fd46c7af?auto=format&fit=crop&w=400&q=70",
        };

        var urunler = db.MenuUrunleri.ToList();
        var degisti = false;
        foreach (var urun in urunler)
        {
            if (fotograflar.TryGetValue(urun.Ad, out var url) && urun.Resim != url)
            {
                urun.Resim = url;
                degisti = true;
            }
        }
        if (degisti) db.SaveChanges();
    }

    // Şu an açık (BitisZamani == null) bir iş günü yoksa yeni bir tane açar,
    // varsa onu döner. Uygulama her açıldığında ve masaya ilk ürün eklendiğinde çağrılır.
    public static IsGunu AktifGunuGetirVeyaOlustur(ApplicationDbContext db)
    {
        var aktif = db.IsGunleri.FirstOrDefault(g => g.BitisZamani == null);
        if (aktif != null) return aktif;

        aktif = new IsGunu { BaslangicZamani = DateTime.Now };
        db.IsGunleri.Add(aktif);
        db.SaveChanges();
        return aktif;
    }
}
