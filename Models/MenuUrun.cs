namespace LostHeavenCafe.Models;

public class MenuUrun
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty; // "Çay", "Kaşarlı Tost", "Menemen"...
    public decimal Fiyat { get; set; }
    // Ya tam bir web adresi (https://...) ya da wwwroot/images/urunler/ altındaki
    // bir dosya adı olabilir — görüntülerken ikisi de otomatik ayırt edilir.
    public string? Resim { get; set; }
    public bool Aktif { get; set; } = true; // menüden geçici kaldırmak için (silmeden)

    public int KategoriId { get; set; }
    public MenuKategori? Kategori { get; set; }
}
