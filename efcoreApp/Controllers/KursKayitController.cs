using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace efcoreApp.Controllers
{
    public class KursKayitController:Controller
    {
        private readonly DataContext _context;

        public KursKayitController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var KursKayitlari = await _context.KursKayitlari
            .Include(x =>x.Ogrenci)
            .Include(x =>x.Kurs).ToListAsync(); //kurs kayıtlarını listeler(Ogrenci ve Kurs tablosundaki her alana erişmeyi sağlar)
            return View(KursKayitlari); //sayfaya gönderir.
        }

        [HttpGet]
        public async Task<IActionResult> Create() //kayıt oluşturma
        {
            ViewBag.Ogrenciler = new SelectList(await _context.Ogrenciler.ToListAsync(),"Id","AdSoyad"); //Öğrenci listesini Viewbag e gönderir ve sayfadaki kurs kayıt kısmına gönderir.

            ViewBag.Kurslar = new SelectList(await _context.Kurslar.ToListAsync(),"KursId","Baslik");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursKayit model) //kurskayit model bilgisini alarak kayıt oluşturur.
        {
            model.KayitTarihi = DateTime.Now; //kayıt tarihi bilgisini(modelden) alır.
            _context.KursKayitlari.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}