using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LostHeavenCafe.Models;

namespace LostHeavenCafe.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string sifre)
    {
        var sonuc = await _signInManager.PasswordSignInAsync(email, sifre, isPersistent: true, lockoutOnFailure: false);
        if (sonuc.Succeeded)
            return RedirectToAction("Index", "Masalar");

        ModelState.AddModelError("", "E-posta veya şifre hatalı.");
        return View();
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string adSoyad, string email, string sifre)
    {
        var kullanici = new ApplicationUser { UserName = email, Email = email, AdSoyad = adSoyad };
        var sonuc = await _userManager.CreateAsync(kullanici, sifre);

        if (sonuc.Succeeded)
        {
            await _signInManager.SignInAsync(kullanici, isPersistent: true);
            return RedirectToAction("Index", "Masalar");
        }

        foreach (var hata in sonuc.Errors)
            ModelState.AddModelError("", hata.Description);

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Menu");
    }
}
