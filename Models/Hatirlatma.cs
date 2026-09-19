namespace LostHeavenCafe.Models;

// Basit bir not/hatırlatma: "Toptancı Salı geliyor", "Peçete sipariş et" gibi
// tarihe bağlı işleri takip etmek için. Takvim değil, tarihe göre sıralı bir liste.
public class Hatirlatma
{
    public int Id { get; set; }
    public DateTime Tarih { get; set; }
    public string Baslik { get; set; } = string.Empty;
    public string? Not { get; set; }
    public bool TamamlandiMi { get; set; }
}
