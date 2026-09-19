namespace LostHeavenCafe.Models;

public class Masa
{
    public int Id { get; set; }
    public int MasaNo { get; set; } // 1..30

    // Şu an açık bir adisyonu var mı — grid ekranında rengi bu belirler.
    public bool Dolu { get; set; }
}
