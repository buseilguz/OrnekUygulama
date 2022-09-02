using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrnekUygulama.Models;

namespace OrnekUygulama.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        yemektarifleriContext db = new yemektarifleriContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int id)
        {
            
            var sayfa = db.Sayfalars.Where(a=> a.Aktif == true && a.Silindi==false && a.SayfaId==id).FirstOrDefault();

            return View(sayfa);
        }
        public IActionResult TumTarifler()
        {
            
            var tarifler = db.Yemektarifleris.Include(k=>k.Kategori).Where(a => a.Aktif == true && a.Silindi == false ).OrderBy(s=>s.Sira).ToList();

            return View(tarifler);
        }

        public IActionResult Tarif(int id)
        {
            TarifYorumlar t = new TarifYorumlar();
            var tarif= db.Yemektarifleris.Include(k => k.Kategori).Where(a => a.Aktif == true && a.Silindi == false&& a.TarifId==id).FirstOrDefault();
            t.tarif = tarif;
            var yorumlar = db.Yorumlars.Include(u=>u.Uye).Include(y=>y.Tarif).Where(y => y.Silindi == false && y.Aktif == true && y.TarifId == id).OrderByDescending(y=>y.Eklemetarihi).ToList();
            t.yorumlar = yorumlar;
            return View(t);
        }
        public IActionResult YorumYap(Yorumlar yor)
        {
            yor.Silindi = false;
            yor.Aktif = false;
            yor.Eklemetarihi = DateTime.Now;
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            yor.KullaniciId = Convert.ToInt32(id);
            db.Yorumlars.Add(yor);
            db.SaveChanges();
            TempData["mesaj"] = "Yorumunuz alındı, yönetici onayından sonra görünecektir.";
            return Redirect("/Home/Tarif/" +yor.TarifId);
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
