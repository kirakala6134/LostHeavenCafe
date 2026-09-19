namespace LostHeavenCafe.Models;

public class AdisyonKalemi
{
    public int Id { get; set; }

    public int AdisyonId { get; set; }
    public Adisyon? Adisyon { get; set; }

    public int UrunId { get; set; }
    public MenuUrun? Urun { get; set; }

    public int Adet { get; set; }

    // Ürün eklendiği andaki fiyat — menüde fiyat sonradan değişse bile
    // geçmiş adisyonların tutarı bozulmasın diye burada ayrıca tutuluyor.
    public decimal BirimFiyat { get; set; }
}
