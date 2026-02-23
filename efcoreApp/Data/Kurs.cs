namespace efcoreApp.Data //Kurs.cs entity sınıfını tanımlar.
{
    public class Kurs
    {
        public int KursId { get; set; }
        public string? Baslik { get; set; } //Kurs başlığı

         public int? OgretmenId { get; set; }
         public Ogretmen Ogretmen { get; set; } = null!;
        public ICollection<KursKayit>KursKayitları{get; set;} = new List<KursKayit>();
    }
}