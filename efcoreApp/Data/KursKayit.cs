using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Data //KursKayit.cs entity sınıfını tanımlar.
{
    public class KursKayit
    {

     [Key] //KayitId birincil anahtar olarak tanımlanmıştır.
     public int KayitId { get; set; }
     public int OgrenciId { get; set; } //Yabancı anahtar olarak OgrenciId tanımlanmıştır.
     public Ogrenci Ogrenci {get; set;} = null!; //öğrenciler tablosundaki öğrencilere erişmeyi sağlar(join işlemleri)
     public int KursId { get; set; } 
     public Kurs Kurs{get; set;} = null!; //Kurslar tablosundaki kurslara erişmeyi sağlar(join)
     public DateTime KayitTarihi { get; set; }

    }

}
//örnek: 1 numaralı kayıt, 2 numaralı öğrenci 3 numaralı kursa kayıtlı.