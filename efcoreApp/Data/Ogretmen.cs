using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace efcoreApp.Data
{
    public class Ogretmen
    {
        [Key]
        public int OgretmenId { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
         public string AdSoyad{get //Öğrenci ad ve soyad bilgisini birleştirerek alır.
            {
             return this.Ad + " " + this.Soyad; 
            }
        }
        public string? Eposta { get; set; }
        public string? Telefon { get; set; }

        [DataType(DataType.Date)] //sadece tarih bilgisini tutmak istediğimiz için DataType ekledik.
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)] //tarih formatını belirledik.
        public DateTime BaslamaTarihi { get; set; }
        public ICollection<Kurs>Kurslar = new List<Kurs>(); //bir öğretmen birden fazla kurs da görev alabilir(özellik ekledik)
    }
}