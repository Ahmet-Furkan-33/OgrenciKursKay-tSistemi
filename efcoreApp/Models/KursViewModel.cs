using efcoreApp.Data;
namespace efcoreApp.Models //KursViewModel sınıfını tanımlar. Bu sınıf, Kurs ve Ogretmen bilgilerini bir arada tutmak için kullanılır.
{
    public class KursViewModel
    {
        public int KursId { get; set; }
        public string? Baslik { get; set; } //Kurs başlığı

         public int? OgretmenId { get; set; }

          public ICollection<KursKayit>KursKayitları{get; set;} = new List<KursKayit>(); //Kursa kayıtlı öğrencilerin bilgilerini(modelde) tutar.
    }
}