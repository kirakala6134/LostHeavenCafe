using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LostHeavenCafe.Data;
using LostHeavenCafe.Models;

namespace LostHeavenCafe.Controllers;

public class KasaGorunumu
{
    public decimal AnaToplam { get; set; }
    public decimal GunlukToplam { get; set; }
    public DateTime AktifGunBaslangici { get; set; }
    public List<Hatirlatma> YaklasanHatirlatmalar { get; set; } = new();
}

[Authorize]
public class KasaController : Controller
{
    private readonly ApplicationDbContext _db;
    public KasaController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var aktifGun = ApplicationDbContext.AktifGunuGetirVeyaOlustur(_db);

        // SQLite decimal üzerinde sunucu tarafı Sum'ı desteklemiyor,
        // bu yüzden ilgili kalemleri çekip toplamı bellekte hesaplıyoruz.
        var kapaliKalemler = await _db.AdisyonKalemleri
            .Where(k => k.Adisyon!.Kapali)
            .Select(k => new { k.Adet, k.BirimFiyat, k.Adisyon!.IsGunuId })
            .ToListAsync();

        var anaToplam = kapaliKalemler.Sum(k => k.Adet * k.BirimFiyat);
        var gunlukToplam = kapaliKalemler.Where(k => k.IsGunuId == aktifGun.Id).Sum(k => k.Adet * k.BirimFiyat);

        return View(new KasaGorunumu
        {
            AnaToplam = anaToplam,
            GunlukToplam = gunlukToplam,
            AktifGunBaslangici = aktifGun.BaslangicZamani,
            YaklasanHatirlatmalar = await _db.Hatirlatmalar
                .Where(h => !h.TamamlandiMi)
                .OrderBy(h => h.Tarih)
                .Take(3)
                .ToListAsync()
        });
    }

    // Aktif iş gününü kapatır, yenisini açar. Ana toplam etkilenmez,
    // günlük toplam yeni günle birlikte sıfırdan başlar.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult GunuBitir()
    {
        var aktifGun = ApplicationDbContext.AktifGunuGetirVeyaOlustur(_db);
        aktifGun.BitisZamani = DateTime.Now;
        _db.SaveChanges();

        // Yeni günü hemen aç, bir sonraki satış otomatik ona bağlansın.
        ApplicationDbContext.AktifGunuGetirVeyaOlustur(_db);

        return RedirectToAction("Index");
    }
}
