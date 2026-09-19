using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LostHeavenCafe.Data;
using LostHeavenCafe.Models;

namespace LostHeavenCafe.Controllers;

public class MenuController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;

    public MenuController(ApplicationDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // Herkese açık — QR kod bu sayfaya yönlendirilecek. Giriş gerekmez.
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var kategoriler = await _db.MenuKategorileri
            .Include(k => k.Urunler.Where(u => u.Aktif))
            .OrderBy(k => k.Sira)
            .ToListAsync();
        return View(kategoriler);
    }

    // QR kod üretme ekranı — hangi adrese QR üreteceğini istediğin zaman
    // buradan değiştirebilirsin (localhost'ta test için, ya da gerçek alan
    // adına geçtiğinde tekrar buraya gelip yeni QR üretirsin).
    [Authorize]
    public IActionResult QrOlustur()
    {
        var varsayilanAdres = Url.Action("Index", "Menu", null, Request.Scheme);
        ViewBag.VarsayilanAdres = varsayilanAdres;
        return View();
    }

    // --- Aşağısı sadece işletmeci için: menüyü düzenleme ---
    [Authorize]
    public async Task<IActionResult> Yonet()
    {
        var kategoriler = await _db.MenuKategorileri
            .Include(k => k.Urunler)
            .OrderBy(k => k.Sira)
            .ToListAsync();
        return View(kategoriler);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UrunEkle(int kategoriId, string ad, decimal fiyat, string? resimUrl, IFormFile? resimDosyasi)
    {
        var urun = new MenuUrun { KategoriId = kategoriId, Ad = ad, Fiyat = fiyat, Aktif = true };
        urun.Resim = await ResimKaydet(resimDosyasi, resimUrl) ?? urun.Resim;

        _db.MenuUrunleri.Add(urun);
        await _db.SaveChangesAsync();
        return RedirectToAction("Yonet");
    }

    // Var olan bir ürünün resmini sonradan eklemek/değiştirmek için.
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UrunResimGuncelle(int urunId, string? resimUrl, IFormFile? resimDosyasi)
    {
        var urun = await _db.MenuUrunleri.FindAsync(urunId);
        if (urun != null)
        {
            var yeniResim = await ResimKaydet(resimDosyasi, resimUrl);
            if (yeniResim != null)
            {
                urun.Resim = yeniResim;
                await _db.SaveChangesAsync();
            }
        }
        return RedirectToAction("Yonet");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UrunDurumDegistir(int urunId)
    {
        var urun = await _db.MenuUrunleri.FindAsync(urunId);
        if (urun != null)
        {
            urun.Aktif = !urun.Aktif; // menüden geçici kaldır / geri getir
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Yonet");
    }

    // Ürünü veritabanından tamamen siler. Bu ürün geçmişte bir masada
    // satılmışsa (AdisyonKalemi'nde kaydı varsa) geçmiş ciro rakamları
    // bozulmasın diye silmeyi reddeder — o durumda "Pasif Yap" kullanılmalı.
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UrunSil(int urunId)
    {
        var gecmisteSatilmis = await _db.AdisyonKalemleri.AnyAsync(k => k.UrunId == urunId);
        if (gecmisteSatilmis)
        {
            TempData["HataMesaji"] = "Bu ürün geçmişte satılmış, tamamen silinemez. Bunun yerine \"Pasif Yap\" kullan.";
            return RedirectToAction("Yonet");
        }

        var urun = await _db.MenuUrunleri.FindAsync(urunId);
        if (urun != null)
        {
            _db.MenuUrunleri.Remove(urun);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Yonet");
    }

    // Ya yüklenen dosyayı wwwroot/images/urunler/ altına kaydedip dosya adını,
    // ya da (dosya yoksa) yazılan URL'yi döner. İkisi de boşsa null döner.
    private async Task<string?> ResimKaydet(IFormFile? dosya, string? url)
    {
        if (dosya is { Length: > 0 })
        {
            var uzanti = Path.GetExtension(dosya.FileName);
            var dosyaAdi = $"{Guid.NewGuid()}{uzanti}";
            var klasor = Path.Combine(_env.WebRootPath, "images", "urunler");
            Directory.CreateDirectory(klasor);

            var tamYol = Path.Combine(klasor, dosyaAdi);
            using var stream = System.IO.File.Create(tamYol);
            await dosya.CopyToAsync(stream);

            return dosyaAdi;
        }

        if (!string.IsNullOrWhiteSpace(url))
            return url.Trim();

        return null;
    }
}

