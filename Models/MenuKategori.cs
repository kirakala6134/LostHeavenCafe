namespace LostHeavenCafe.Models;

public class MenuKategori
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty; // "Çaylar", "Tostlar", "Kahvaltı"...
    public int Sira { get; set; } // menüde gösterim sırası

    public List<MenuUrun> Urunler { get; set; } = new();
}
