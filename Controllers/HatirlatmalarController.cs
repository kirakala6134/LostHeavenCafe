using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LostHeavenCafe.Data;
using LostHeavenCafe.Models;

namespace LostHeavenCafe.Controllers;

[Authorize]
public class HatirlatmalarController : Controller
{
    private readonly ApplicationDbContext _db;
    public HatirlatmalarController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var hatirlatmalar = await _db.Hatirlatmalar
            .OrderBy(h => h.TamamlandiMi) // tamamlanmayanlar üstte
            .ThenBy(h => h.Tarih)
            .ToListAsync();
        return View(hatirlatmalar);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(DateTime tarih, string baslik, string? not)
    {
        _db.Hatirlatmalar.Add(new Hatirlatma { Tarih = tarih, Baslik = baslik, Not = not, TamamlandiMi = false });
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TamamlandiIsaretle(int id)
    {
        var h = await _db.Hatirlatmalar.FindAsync(id);
        if (h != null)
        {
            h.TamamlandiMi = !h.TamamlandiMi;
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var h = await _db.Hatirlatmalar.FindAsync(id);
        if (h != null)
        {
            _db.Hatirlatmalar.Remove(h);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
