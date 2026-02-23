using Microsoft.EntityFrameworkCore; //Entity Framework Core kütüphanesini kullanır.
using Microsoft.Net.Http.Headers;

namespace efcoreApp.Data 
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) 
        { // DbContext için gerekli ayarları (connection string vb.) alır ve base DbContext'e iletir
            
        }
        public DbSet<Kurs> Kurslar => Set<Kurs>();  // Veritabanındaki Kurs tablosunu temsil eden DbSet özelliği

        public DbSet<Ogrenci> Ogrenciler => Set<Ogrenci>(); 

        public DbSet<KursKayit> KursKayitlari => Set<KursKayit>(); 

         public DbSet<Ogretmen> Ogretmenler => Set<Ogretmen>(); 

    }
    
}