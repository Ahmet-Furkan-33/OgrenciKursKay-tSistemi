using System.Runtime.CompilerServices;
using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class OgrenciController:Controller
    {
        private readonly DataContext _context; // Controller içinde kullanılacak veritabanı context'i tanımlar, readonly olduğu için sadece constructor’da atanır
        public OgrenciController(DataContext context)
        {
            _context = context; // Dışarıdan (Dependency Injection ile) gelen DataContext nesnesini sınıf içindeki _context alanına atar
        }
        public async Task<IActionResult> List() //listeleme sayfası
        {
            return View(await _context.Ogrenciler.ToListAsync()); // Ogrenciler tablosundaki tüm verileri asenkron olarak çekip View’a gönderir
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci model) //öğrenci ekleme sayfası
        {
            _context.Ogrenciler.Add(model);  // Ogrenciler tablosuna yeni bir öğrenci kaydı ekler.
            await _context.SaveChangesAsync(); // Yapılan değişiklikleri veritabanına kaydeder.
            return RedirectToAction("List"); // İşlem tamamlandıktan sonra HomeController içindeki Index action’ına yönlendirir.

            
        }
        [HttpGet] // Bu metodun HTTP GET isteği ile çalışacağını belirtir.
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            
            // Veritabanındaki Ogrenciler tablosundan, ilgili öğrenciyi ve ilişkili olduğu KursKayitları ile o kayıtların bağlı olduğu Kurs bilgilerini birlikte getirir.                  
            var ogr = await _context
            .Ogrenciler
            .Include(o => o.KursKayitları) // Öğrencinin KursKayitları koleksiyonunu dahil eder (eager loading)
            .ThenInclude(o => o.Kurs) // KursKayitları içindeki Kurs nesnesini de dahil eder
            .FirstOrDefaultAsync(o => o.Id == id); // Id'si verilen değere eşit olan ilk öğrenciyi getirir

            if(ogr == null) //öğrenci yoksa
            {
                return NotFound(); //hata sayfası döner.
            }

            return View(ogr); //sayfada öğrenci bilgisini gösterir.
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik önlemi
        public async Task<IActionResult> Edit(int id, Ogrenci model) //kayıt id si ve formdan gelen id bilgisini alıp güncelleyecek.
        {
            if(id !=model.Id) //eğer kayıt id si ile modelden gelen id bilgisi eşleşmiyorsa
            {
                return NotFound(); //hata sayfası döner.
            }
            
            if(ModelState.IsValid) //formdan gelen bilgiler eşleşiyorsa günceller
            { 
                try
                {

                _context.Update(model); //Öğrenciler tablosundaki kaydı günceller.
                await _context.SaveChangesAsync(); //Yapılan değişiklikleri veritabanına kaydeder.

                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!_context.Ogrenciler.Any(o => o.Id == model.Id)) //veri tabanında hiç eşleşen kayıt yoksa
                    {
                        return NotFound(); //hata sayfası döner.
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("List");// güncelleme tamamlandıktan sonra HomeController içindeki List action’ına yönlendirir.
            }

            return View(model); //yoksa modelden gelen bilgileri sayfaya tekrar gönderir.
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var ogrenci = await _context.Ogrenciler.FindAsync(id); //Ogrenci bilgisini alır .

            if(ogrenci == null)
            {
                return NotFound();
            }
            return View(ogrenci); //öğrenci modelini sayfaya gönderir.
        } 
        
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id) //formdan gelen id bilgisine göre silme
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id); //öğrenci bilgisi alır.
            if(ogrenci == null)
            {
                return NotFound();
            }
            _context.Ogrenciler.Remove(ogrenci); //id bilgisine göre öğrenciyi siler.
            await _context.SaveChangesAsync(); // veri tabanına değişiklikleri kaydeder.
            return RedirectToAction("List");
        }
    }
}

//var ogr = await _context.Ogrenciler.FirstOrDefaultAsync(x => x.Id == id)  (diğer arama yolu)