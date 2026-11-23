using Microsoft.AspNetCore.Identity;

namespace LostHeavenCafe.Models;

// Garson/işletmeci paneline giriş yapan kullanıcı.
public class ApplicationUser : IdentityUser
{
    public string? AdSoyad { get; set; }
}
