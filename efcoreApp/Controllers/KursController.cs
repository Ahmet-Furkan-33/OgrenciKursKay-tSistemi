using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class KursController:Controller
    {
        private readonly DataContext _context;
        public KursController(DataContext context)
        {
            _context = context;  // Dışarıdan (Dependency Injection ile) gelen DataContext nesnesini sınıf içindeki _context alanına atar
        }

        public async Task<IActionResult> List() //kurs liste sayfası
        {
            var kurslar = await _context.Kurslar.Include(k => k.Ogretmen).ToListAsync(); //kurslar bilgisini öğretmen bilgisiyle birlikte alır.
            return View(kurslar); //kurslar bilgisini kurs list sayfasına gönderir.
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","AdSoyad"); //ogretmenler listesini selectlist olarak viewbag e atar.

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Kurs model) //kurs modelinin bilgisini alır oluştumak için
        {
             _context.Kurslar.Add(model); //modelden gelen bilgileri kurslar a ekler.
             await _context.SaveChangesAsync(); //veritabanına değişiklikleri kaydeder.
             return View("List"); //işlem tamamlanınca /kurs/Index sayfasına gönderir.
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var kurslar = await _context
                                .Kurslar
                                .Include(k =>k.KursKayitları) 
                                .ThenInclude(k =>k.Ogrenci)
                                .Select(k => new KursViewModel() //seçilen kursun bilgilerini  yeni oluşturulan KursViewModel sınıfına dönüştürerek alır. 
                                {
                                    KursId = k.KursId, //kurs id bilgisi seçer.
                                    Baslik = k.Baslik,
                                    OgretmenId = k.OgretmenId,
                                    KursKayitları = k.KursKayitları
                                })
                                .FirstOrDefaultAsync(k=>k.KursId == id);
            if(kurslar == null)
            {
                return NotFound();
            }
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","AdSoyad"); //   ogretmenler listesini selectlist olarak viewbag e atar.(öğretmen değiştirmek için)
             return View(kurslar); //kurs bilgisiyle edit sayfasını açar.
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KursViewModel model)
        {
            if(id != model.KursId)
            {
                return NotFound();
            }
            
            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Kurs() {KursId =model.KursId, Baslik = model.Baslik, OgretmenId = model.OgretmenId});//kurs bilgilerini günceller.
                        
                    
                    await _context.SaveChangesAsync(); //değişiklikleri kaydeder.
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!_context.Kurslar.Any(x =>x.KursId == model.KursId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("List");
            }
            return View(model); //kurs bilgisi çıkmazsa sayfaya modeli geri gönderir.
        }
          [HttpGet]
        public async Task<IActionResult> Delete(int? id) //kurs silme get işlemi
        {
            if(id == null)
            {
                return NotFound();
            }
            var kurs = await _context.Kurslar.FindAsync(id); //kurs bilgisini alır .

            if(kurs == null)
            {
                return NotFound();
            }
            return View(kurs); //kurs modelini sayfaya gönderir.
        } 
        
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id) //formdan gelen id bilgisine göre silme
        {
            var kurs = await _context.Kurslar.FindAsync(id); //kurs bilgisi alır.
            if(kurs == null)
            {
                return NotFound();
            }
            _context.Kurslar.Remove(kurs); //id bilgisine göre kursu siler.
            await _context.SaveChangesAsync(); // veri tabanına değişiklikleri kaydeder.
            return RedirectToAction("List"); //kurscontroller altındaki list sayfasına gönderir.
        }
    }
}