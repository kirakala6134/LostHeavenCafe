using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LostHeavenCafe.Controllers;

// Herkese açık karşılama sayfası — Giriş Yap / Kayıt Ol panelleri burada.
[AllowAnonymous]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        // Zaten giriş yapmışsa doğrudan panele gönder, tekrar karşılama görmesin.
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Masalar");

        return View();
    }
}
