using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LostHeavenCafe.Data;
using LostHeavenCafe.Models;

namespace LostHeavenCafe.Controllers;

[Authorize]
public class MasalarController : Controller
{
    private readonly ApplicationDbContext _db;
    public MasalarController(ApplicationDbContext db) => _db = db;

    // 30 masanın dolu/boş durumunu gösteren ana grid.
    public async Task<IActionResult> Index()
    {
        var masalar = await _db.Masalar.OrderBy(m => m.MasaNo).ToListAsync();
        return View(masalar);
    }

    // Bir masaya tıklanınca: mevcut açık adisyonu (varsa) ve menüyü göster.
    public async Task<IActionResult> Detay(int masaNo)
    {
        var masa = await _db.Masalar.FirstAsync(m => m.MasaNo == masaNo);

        var adisyon = await _db.Adisyonlar
            .Include(a => a.Kalemler).ThenInclude(k => k.Urun)
            .Where(a => a.MasaId == masa.Id && !a.Kapali)
            .FirstOrDefaultAsync();

        var kategoriler = await _db.MenuKategorileri
            .Include(k => k.Urunler.Where(u => u.Aktif))
            .OrderBy(k => k.Sira)
            .ToListAsync();

        ViewBag.Masa = masa;
        ViewBag.Kategoriler = kategoriler;
        return View(adisyon); // adisyon null olabilir: henüz hiç sipariş girilmemiş demektir
    }

    // Masaya ürün ekle (+1). Açık adisyon yoksa otomatik oluşturur.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UrunEkle(int masaNo, int urunId)
    {
        var masa = await _db.Masalar.FirstAsync(m => m.MasaNo == masaNo);

        var adisyon = await _db.Adisyonlar
            .Include(a => a.Kalemler)
            .FirstOrDefaultAsync(a => a.MasaId == masa.Id && !a.Kapali);

        if (adisyon == null)
        {
            var aktifGun = ApplicationDbContext.AktifGunuGetirVeyaOlustur(_db);
            adisyon = new Adisyon { MasaId = masa.Id, AcilisZamani = DateTime.Now, Kapali = false, IsGunuId = aktifGun.Id };
            _db.Adisyonlar.Add(adisyon);
            masa.Dolu = true;
            await _db.SaveChangesAsync();
        }

        var kalem = adisyon.Kalemler.FirstOrDefault(k => k.UrunId == urunId);
        if (kalem == null)
        {
            var urun = await _db.MenuUrunleri.FirstAsync(u => u.Id == urunId);
            _db.AdisyonKalemleri.Add(new AdisyonKalemi
            {
                AdisyonId = adisyon.Id,
                UrunId = urunId,
                Adet = 1,
                BirimFiyat = urun.Fiyat
            });
        }
        else
        {
            kalem.Adet += 1;
        }

        await _db.SaveChangesAsync();
        return RedirectToAction("Detay", new { masaNo });
    }

    // Masadan ürün azalt (-1). 0'a düşerse satırı tamamen kaldırır.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UrunCikar(int masaNo, int urunId)
    {
        var masa = await _db.Masalar.FirstAsync(m => m.MasaNo == masaNo);
        var adisyon = await _db.Adisyonlar
            .Include(a => a.Kalemler)
            .FirstOrDefaultAsync(a => a.MasaId == masa.Id && !a.Kapali);

        var kalem = adisyon?.Kalemler.FirstOrDefault(k => k.UrunId == urunId);
        if (kalem != null)
        {
            kalem.Adet -= 1;
            if (kalem.Adet <= 0)
                _db.AdisyonKalemleri.Remove(kalem);

            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Detay", new { masaNo });
    }

    // Hesabı kapat: ödeme kart ile masada alınıyor, sistemin bununla işi yok —
    // bu buton sadece masayı "boş"a çevirip adisyonu geçmişe kaydediyor.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HesabiKapat(int masaNo)
    {
        var masa = await _db.Masalar.FirstAsync(m => m.MasaNo == masaNo);
        var adisyon = await _db.Adisyonlar.FirstOrDefaultAsync(a => a.MasaId == masa.Id && !a.Kapali);

        if (adisyon != null)
        {
            adisyon.Kapali = true;
            adisyon.KapanisZamani = DateTime.Now;
            masa.Dolu = false;
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}
