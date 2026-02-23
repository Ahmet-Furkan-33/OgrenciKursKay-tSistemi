using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class OgretmenController: Controller
    {
        private readonly DataContext _context;
        public OgretmenController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var Ogretmenler = await _context.Ogretmenler.ToListAsync();
            return View(Ogretmenler);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
          return View();    
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogretmen model)
        {
            _context.Ogretmenler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
         [HttpGet] // Bu metodun HTTP GET isteği ile çalışacağını belirtir.
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            
            // Veritabanındaki Ogretmenler tablosundan, ilgili öğretmeni ve ilişkili olduğu KursKayitları ile o kayıtların bağlı olduğu Kurs bilgilerini birlikte getirir.                  
            var ogr = await _context
            .Ogretmenler
            .FirstOrDefaultAsync(o => o.OgretmenId == id); // Id'si verilen değere eşit olan ilk öğretmeni getirir

            if(ogr == null) //öğretmen yoksa
            {
                return NotFound(); //hata sayfası döner.
            }

            return View(ogr); //sayfada öğretmen bilgisini gösterir.
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik önlemi
        public async Task<IActionResult> Edit(int id, Ogretmen model) //kayıt id si ve formdan gelen id bilgisini alıp güncelleyecek.
        {
            if(id !=model.OgretmenId) //eğer kayıt id si ile modelden gelen id bilgisi eşleşmiyorsa
            {
                return NotFound(); //hata sayfası döner.
            }
            
            if(ModelState.IsValid) //formdan gelen bilgiler eşleşiyorsa günceller
            { 
                try
                {

                _context.Update(model); //Öğretmenler tablosundaki kaydı günceller.
                await _context.SaveChangesAsync(); //Yapılan değişiklikleri veritabanına kaydeder.

                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!_context.Ogretmenler.Any(o => o.OgretmenId == model.OgretmenId)) //veri tabanında hiç eşleşen kayıt yoksa
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
            var ogretmen = await _context.Ogretmenler.FindAsync(id); //Ogretmen bilgisini alır .

            if(ogretmen == null)
            {
                return NotFound();
            }
            return View(ogretmen); //öğretmen modelini sayfaya gönderir.
        } 
        
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id) //formdan gelen id bilgisine göre silme
        {
            var ogretmen = await _context.Ogretmenler.FindAsync(id); //öğretmen bilgisi alır.
            if(ogretmen == null)
            {
                return NotFound();
            }
            _context.Ogretmenler.Remove(ogretmen); //id bilgisine göre öğretmeni siler.
            await _context.SaveChangesAsync(); // veri tabanına değişiklikleri kaydeder.
            return RedirectToAction("List");
        }
    }
}
