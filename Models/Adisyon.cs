namespace LostHeavenCafe.Models;

public class Adisyon
{
    public int Id { get; set; }

    public int MasaId { get; set; }
    public Masa? Masa { get; set; }

    public DateTime AcilisZamani { get; set; }
    public DateTime? KapanisZamani { get; set; }

    public bool Kapali { get; set; } // true = ödeme alınıp kapatıldı (kart ile, sistem dışı)

    // Açıldığı andaki aktif iş günü — günlük toplam hesaplamasında kullanılır.
    public int? IsGunuId { get; set; }
    public IsGunu? IsGunu { get; set; }

    public List<AdisyonKalemi> Kalemler { get; set; } = new();
}
