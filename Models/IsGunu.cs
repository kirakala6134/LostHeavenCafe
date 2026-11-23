namespace LostHeavenCafe.Models;

// "Günü Bitir" butonuyla kapatılıp yenisi açılan iş günü kaydı.
// Günlük toplam bu güne bağlı adisyonlardan hesaplanır, ana toplam ise
// tüm iş günlerinden (hepsi) hesaplanır — biri sıfırlanırken diğeri etkilenmez.
public class IsGunu
{
    public int Id { get; set; }
    public DateTime BaslangicZamani { get; set; }
    public DateTime? BitisZamani { get; set; } // null = hâlâ aktif gün
}
