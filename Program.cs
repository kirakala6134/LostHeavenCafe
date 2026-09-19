using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LostHeavenCafe.Data;
using LostHeavenCafe.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=lostheaven.db"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false; // rakam zorunlu değil, gereksiz katılık
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddErrorDescriber<TurkceHataAciklayici>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Kök adres artık karşılama (Home) sayfası — Giriş/Kayıt panelleri burada.
// QR kodunu /Menu adresine yönlendirmen gerekiyor (bkz. README), çünkü
// müşterinin karşılama/giriş sayfasını görmesine gerek yok.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    ApplicationDbContext.OrnekVeriyiOlustur(db);
    ApplicationDbContext.UrunFotograflariniGuncelle(db);
    ApplicationDbContext.AktifGunuGetirVeyaOlustur(db);
}

app.Run();
