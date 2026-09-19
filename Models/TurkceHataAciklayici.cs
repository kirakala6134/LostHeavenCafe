using Microsoft.AspNetCore.Identity;

namespace LostHeavenCafe.Models;

// ASP.NET Core Identity'nin varsayılan hata mesajları İngilizce geliyor,
// bu sınıf onları Türkçeye çeviriyor.
public class TurkceHataAciklayici : IdentityErrorDescriber
{
    public override IdentityError PasswordTooShort(int length)
        => new() { Code = nameof(PasswordTooShort), Description = $"Şifre en az {length} karakter olmalı." };

    public override IdentityError PasswordRequiresLower()
        => new() { Code = nameof(PasswordRequiresLower), Description = "Şifre en az bir küçük harf içermeli." };

    public override IdentityError PasswordRequiresUpper()
        => new() { Code = nameof(PasswordRequiresUpper), Description = "Şifre en az bir büyük harf içermeli." };

    public override IdentityError PasswordRequiresDigit()
        => new() { Code = nameof(PasswordRequiresDigit), Description = "Şifre en az bir rakam içermeli." };

    public override IdentityError PasswordRequiresNonAlphanumeric()
        => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Şifre en az bir özel karakter içermeli." };

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        => new() { Code = nameof(PasswordRequiresUniqueChars), Description = $"Şifre en az {uniqueChars} farklı karakter içermeli." };

    public override IdentityError DuplicateEmail(string email)
        => new() { Code = nameof(DuplicateEmail), Description = "Bu e-posta adresiyle zaten bir hesap var." };

    public override IdentityError DuplicateUserName(string userName)
        => new() { Code = nameof(DuplicateUserName), Description = "Bu kullanıcı adı zaten kullanılıyor." };

    public override IdentityError InvalidEmail(string? email)
        => new() { Code = nameof(InvalidEmail), Description = "Geçerli bir e-posta adresi gir." };

    public override IdentityError InvalidUserName(string? userName)
        => new() { Code = nameof(InvalidUserName), Description = "Geçersiz kullanıcı adı." };

    public override IdentityError PasswordMismatch()
        => new() { Code = nameof(PasswordMismatch), Description = "Şifre hatalı." };

    public override IdentityError DefaultError()
        => new() { Code = nameof(DefaultError), Description = "Beklenmeyen bir hata oluştu, tekrar dene." };

    public override IdentityError ConcurrencyFailure()
        => new() { Code = nameof(ConcurrencyFailure), Description = "Bu kayıt başka bir işlem tarafından değiştirilmiş, sayfayı yenile." };

    public override IdentityError InvalidToken()
        => new() { Code = nameof(InvalidToken), Description = "Geçersiz veya süresi dolmuş işlem." };
}
