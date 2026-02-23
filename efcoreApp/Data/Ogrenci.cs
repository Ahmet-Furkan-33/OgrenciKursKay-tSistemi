using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;

namespace efcoreApp.Data //Ogrenci.cs entity sınıfını tanımlar.
{
    public class Ogrenci
    {
        [Key]
        public int Id { get; set; } //Id birincil anahtar olarak tanımlanmıştır.
        public string? OgrenciAd { get; set; }
        public string? OgrenciSoyad { get; set; }
        public string AdSoyad{get //Öğrenci ad ve soyad bilgisini birleştirerek alır.
            {
             return this.OgrenciAd + " " + this.OgrenciSoyad; 
            }
        }
        public string? Eposta { get; set; }
        public string? Telefon { get; set; }

        public ICollection<KursKayit>KursKayitları{get;set;} = new List<KursKayit>(); //Her öğrencinin kayıt olduğu kursları görüntülemeyi sağlar.
    }
}