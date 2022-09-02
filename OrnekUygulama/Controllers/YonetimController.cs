using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrnekUygulama.Models;
using static System.Net.Mime.MediaTypeNames;

namespace OrnekUygulama.Controllers
{  
    
    [Authorize(Roles ="Yonetici")]
    public class YonetimController : Controller
    {
        
        yemektarifleriContext db = new yemektarifleriContext();

       
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Bilgilerim()
        {
            int kulid=Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var kullanici = db.Kullanicilars.Find(kulid);
            kullanici.Parola = "";
            return View(kullanici);
        }


        public IActionResult BilgilerimiGuncelle(Kullanicilar kul)
        {
            var kullanici = db.Kullanicilars.Where(s => s.Silindi == false && s.KullaniciId == kul.KullaniciId).FirstOrDefault();
            
            kullanici.Adi = kul.Adi;
            kullanici.Soyadi = kul.Soyadi;
            kullanici.Eposta = kul.Eposta;
            kullanici.Telefon = kul.Telefon;
            if (kul.Parola != null)
            {
                kullanici.Parola = MD5Sifrele(kul.Parola.Trim());
            }

            db.Kullanicilars.Update(kullanici);
            db.SaveChanges();
            return RedirectToAction("Bilgilerim");
        }
        public IActionResult Sayfalar()
        {
            var sayfalar = db.Sayfalars.Where(s => s.Aktif == true && s.Silindi == false).OrderBy(s => s.Baslik).ToList();
            return View(sayfalar);
        }

        public IActionResult SayfaEkle()
        {

            return View();
        }

        [HttpPost]
        public IActionResult SayfaEkle(Sayfalar sayfa)
        {
            sayfa.Silindi = false;
            db.Sayfalars.Add(sayfa);
            db.SaveChanges();
            return RedirectToAction("Sayfalar");
        }

        public IActionResult SayfaGetir(int id)
        {

            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == id).FirstOrDefault();

            return View("SayfaGuncelle", sayfa);
        }
        public IActionResult SayfaGuncelle(Sayfalar syf)
        {

            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == syf.SayfaId).FirstOrDefault();
            sayfa.Baslik = syf.Baslik;
            sayfa.Icerik = syf.Icerik;
            sayfa.Aktif = syf.Aktif;
            sayfa.Silindi = false;
            db.Sayfalars.Update(sayfa);
            db.SaveChanges();
            return RedirectToAction("Sayfalar");
        }


        public IActionResult SayfaSil(int id)
        {
            //silinmiş gibi göstereceğiz
            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == id).FirstOrDefault();

            sayfa.Silindi = true;
            db.Sayfalars.Update(sayfa);
            db.SaveChanges();
            return RedirectToAction("Sayfalar");
        }


        public IActionResult Kategoriler()
        {
            var kategoriler = db.Kategorilers.Where(s => s.Silindi == false).OrderBy(k => k.Kategoriadi).ToList();
            return View(kategoriler);
        }

        public IActionResult KategoriEkle()
        {

            return View();
        }

        [HttpPost]
        public IActionResult KategoriEkle(Kategoriler k)
        {
            k.Silindi = false;
            db.Kategorilers.Add(k);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }

        public IActionResult KategoriGetir(int id)
        {

            var kategori = db.Kategorilers.Where(s => s.Silindi == false && s.KategoriId == id).FirstOrDefault();

            return View("KategoriGuncelle", kategori);
        }
        public IActionResult KategoriGuncelle(Kategoriler kdgr)
        {

            var kategori = db.Kategorilers.Where(s => s.Silindi == false && s.KategoriId == kdgr.KategoriId).FirstOrDefault();
            kategori.Kategoriadi = kdgr.Kategoriadi;
            kategori.Aktif = kdgr.Aktif;
            kategori.Silindi = false;
            db.Kategorilers.Update(kategori);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }


        public IActionResult KategoriSil(int id)
        {
            //silinmiş gibi göstereceğiz
            var kategori = db.Kategorilers.Where(s => s.Silindi == false && s.KategoriId == id).FirstOrDefault();

            kategori.Silindi = true;
            db.Kategorilers.Update(kategori);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }


        public IActionResult KategoriYemekler(int id)
        {

            var yemekler = db.Yemektarifleris.Include(k => k.Kategori).Where(s => s.Silindi == false && s.Kategoriid == id).ToList();

            return View("Tarifler", yemekler);
        }




        public IActionResult Tarifler()
        {
            var tarifler = db.Yemektarifleris.Include(k => k.Kategori).Where(s => s.Silindi == false).OrderBy(k => k.TarifId).ToList();
            return View(tarifler);
        }

        public IActionResult TarifEkle()
        {
            var kategoriler = (from k in db.Kategorilers.Where(k => k.Silindi == false && k.Aktif == true).ToList()
                               select new SelectListItem
                               {
                                   Text = k.Kategoriadi,
                                   Value = k.KategoriId.ToString()
                               });
            ViewBag.KategoriId = kategoriler;
            return View();
        }

        [HttpPost]
        public IActionResult TarifEkle(Yemektarifleri k)
        {
            k.Silindi = false;
            db.Yemektarifleris.Add(k);
            db.SaveChanges();
            return RedirectToAction("Tarifler");
        }

        public IActionResult TarifGetir(int id)
        {

            var tarif = db.Yemektarifleris.Include(k => k.Kategori).Where(s => s.Silindi == false && s.TarifId == id).FirstOrDefault();
            var kategoriler = (from k in db.Kategorilers.Where(k => k.Silindi == false && k.Aktif == true).ToList()
                               select new SelectListItem
                               {
                                   Text = k.Kategoriadi,
                                   Value = k.KategoriId.ToString()
                               }
                             );
            ViewBag.KategoriId = kategoriler;
            return View("TarifGuncelle", tarif);
        }



        public IActionResult TarifYorumlari(int id)
        {

            var yorumlar = db.Yorumlars.Where(s => s.Silindi == false && s.YorumId == id).ToList();

            db.SaveChanges();
            return View("Yorumlar", yorumlar);
        }





        public IActionResult TarifGuncelle(Yemektarifleri trf)
        {

            var tarif = db.Yemektarifleris.Where(t => t.Silindi == false && t.TarifId == trf.TarifId).FirstOrDefault();
            tarif.Yemekadi = trf.Yemekadi;
            tarif.Tarif = trf.Tarif;
            tarif.Aktif = trf.Aktif;
            tarif.Sira = trf.Sira;
            tarif.Kategoriid = trf.Kategoriid;
            tarif.Silindi = false;
            db.Yemektarifleris.Update(tarif);
            db.SaveChanges();
            return RedirectToAction("Tarifler");
        }
        public IActionResult TarifSil(int id)
        {
            //silinmiş gibi göstereceğiz
            var tarif = db.Yemektarifleris.Where(s => s.Silindi == false && s.TarifId == id).FirstOrDefault();

            tarif.Silindi = true;
            db.Yemektarifleris.Update(tarif);
            db.SaveChanges();
            return RedirectToAction("Tarifler");
        }
        [HttpGet]
        public IActionResult Yorumlar()
        {
            var yorumlar = db.Yorumlars.Include(y => y.Tarif).Include(u => u.Uye).Where(y => y.Silindi == false).OrderByDescending(y =>
            y.Eklemetarihi).ToList();
            return View(yorumlar);
        }


        [HttpPost]
        public IActionResult Yorumlar(string listelemeturu)
        {
            var yorumlar = db.Yorumlars.Include(y => y.Tarif).Include(u => u.Uye).Where(y => y.Silindi == false).OrderBy(y =>
              y.YorumId).ToList();
            switch (listelemeturu)
            {
                case "Onayli":
                    yorumlar = db.Yorumlars.Include(y => y.Tarif).Include(u => u.Uye).Where(y => y.Silindi == false && y.Aktif == true).OrderBy(y =>
                       y.YorumId).ToList();
                    break;
                case "Onaysiz":
                    yorumlar = db.Yorumlars.Include(y => y.Tarif).Include(u => u.Uye).Where(y => y.Silindi == false && y.Aktif == false).OrderBy(y =>
                   y.YorumId).ToList();
                    break;
            }
            return View(yorumlar);
        }


        public IActionResult Onayla(int id)
        {

            var yorum = db.Yorumlars.Include(y => y.Tarif).Include(u => u.Uye).Where(s => s.Silindi == false && s.YorumId == id).FirstOrDefault();

            yorum.Aktif = Convert.ToBoolean((-1 * Convert.ToInt32(yorum.Aktif)) + 1);

            db.Yorumlars.Update(yorum);
            db.SaveChanges();
            return RedirectToAction("Yorumlar");
        }


        public IActionResult YorumSil(int id)
        {

            var yorum = db.Yorumlars.Include(y => y.Tarif).Include(u => u.Uye).Where(s => s.Silindi == false && s.YorumId == id).FirstOrDefault();

            yorum.Silindi = true;

            db.Yorumlars.Update(yorum);
            db.SaveChanges();
            return RedirectToAction("Yorumlar");
        }









        public IActionResult Kullanicilar()
        {
            var kullanicilar = db.Kullanicilars.Where(s => s.Silindi == false).OrderBy(k => k.KullaniciId).ToList();
            return View(kullanicilar);
        }

        public IActionResult KullaniciEkle()
        {

            return View();
        }
        [HttpPost]
        public IActionResult KullaniciEkle(Kullanicilar k)
        {
            k.Silindi = false;
            k.Parola = MD5Sifrele(k.Parola);
            db.Kullanicilars.Add(k);
            db.SaveChanges();
            return RedirectToAction("Kullanicilar");
        }

      
        public IActionResult KullaniciGetir(int id)
        {
            var kullanici = db.Kullanicilars.Where(k => k.Silindi == false && k.KullaniciId == id).FirstOrDefault();
            kullanici.Parola = "";
            return View("KullaniciGuncelle",kullanici);
        }

        public IActionResult KullaniciGuncelle(Kullanicilar kul)
        {

            var kullanici = db.Kullanicilars.Where(s => s.Silindi == false && s.KullaniciId == kul.KullaniciId).FirstOrDefault();
            kullanici.Aktif = kul.Aktif;
            kullanici.Adi = kul.Adi;
            kullanici.Soyadi = kul.Soyadi;
            kullanici.Eposta = kul.Eposta;
            kullanici.Telefon = kul.Telefon;
            kullanici.Yetki = kul.Yetki;
            if (kul.Parola!=null)
            {
                kullanici.Parola = MD5Sifrele(kul.Parola.Trim());
            }

            db.Kullanicilars.Update(kullanici);
            db.SaveChanges();
            return RedirectToAction("Kullanicilar");
        }

       
    

    public IActionResult KullaniciSil(int id)
        {

            var kullanici = db.Kullanicilars.Where(s => s.Silindi == false && s.KullaniciId == id).FirstOrDefault();
            kullanici.Silindi = true;
            db.Update(kullanici);
            db.SaveChanges();
            return RedirectToAction("Kullanicilar");
        }





       



        public IActionResult CikisYap()
        {
            return View();
        }




















        public IActionResult Menuler()
        {
            var menuler = db.Menulers.Where(s => s.Silindi == false).OrderBy(s => s.Baslik).ToList();
            return View(menuler);
        }

        public IActionResult MenuEkle()
        {
            IEnumerable<SelectListItem> menuler = (from k in db.Menulers.Where(k => k.Silindi == false && k.Aktif == true&&k.Ustid==null).ToList()
                               select new SelectListItem
                               {
                                   Text = k.Baslik,
                                   Value = k.MenuId.ToString()
                               }
                             );
            var m2 = new List<SelectListItem>()
            {
                new SelectListItem(" Yok","0")
            };
            menuler.Union(m2);
            ViewBag.ustmenuler = menuler.Union(m2).OrderBy(t => t.Text);
            var sayfalar = (from k in db.Sayfalars.Where(k => k.Silindi == false && k.Aktif == true).OrderBy(t => t.Baslik).ToList()
                            select new SelectListItem
                            {
                                Text = k.Baslik,
                                Value = k.SayfaId.ToString()
                            }
                             ); 
            ViewBag.sayfalar = sayfalar;
            return View();
        }

        [HttpPost]
        public IActionResult MenuEkle(Menuler m)
        {
            if (m.Ustid==0)
            {
                m.Ustid = null;
            }
         
            m.Silindi = false;
            db.Menulers.Add(m);
            db.SaveChanges();
            return RedirectToAction("Menuler");

        }

        public IActionResult MenuGetir(int id)
        {
            var menuler = (from k in db.Menulers.Where(k => k.Silindi == false && k.Aktif == true && k.Ustid == null).ToList()
                           select new SelectListItem
                           {
                               Text = k.Baslik,
                               Value = k.MenuId.ToString()


                           });
           
                              
            var m2 = new List<SelectListItem>()
            {
                new SelectListItem(" Yok","0")
            };
            
            ViewBag.ustmenuler = menuler.Union(m2).OrderBy(t => t.Text);
            var sayfalar = (from k in db.Sayfalars.Where(k => k.Silindi == false && k.Aktif == true).OrderBy(t => t.Baslik).ToList()
                            select new SelectListItem
                            {
                                Text = k.Baslik,
                                Value = k.SayfaId.ToString()
                            }
                             );
            ViewBag.sayfalar = sayfalar;
            var menu = db.Menulers.Where(m => m.Silindi == false && m.MenuId == id).FirstOrDefault();
            return View("MenuGuncelle", menu);
        }
        public IActionResult MenuGuncelle(Menuler mnu)
        {
          

            var menu = db.Menulers.Where(k => k.Silindi == false && k.MenuId == mnu.MenuId).FirstOrDefault();
            menu.Baslik = mnu.Baslik;
            menu.Url = mnu.Url;
            menu.Ustid = mnu.Ustid;
            if (mnu.Ustid == 0)
            {
                menu.Ustid = null;
            }
            menu.Sira =mnu.Sira;
            menu.Aktif = mnu.Aktif;
            menu.Silindi = false;
            db.Menulers.Update(menu);
            db.SaveChanges();
            return RedirectToAction("Menuler");
        }


        public IActionResult MenuSil(int id)
        {
           
            var menu = db.Menulers.Where(s => s.Silindi == false && s.MenuId == id).FirstOrDefault();

            menu.Silindi = true;
            db.Menulers.Update(menu);
            db.SaveChanges();
            return RedirectToAction("Menuler");
        }


        


        public static string MD5Sifrele(string sifrelenecekMetin)
    {

        // MD5CryptoServiceProvider sınıfının bir örneğini oluşturduk.
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        //Parametre olarak gelen veriyi byte dizisine dönüştürdük.
        byte[] dizi = Encoding.UTF8.GetBytes(sifrelenecekMetin);
        //dizinin hash'ini hesaplattık.
        dizi = md5.ComputeHash(dizi);
        //Hashlenmiş verileri depolamak için StringBuilder nesnesi oluşturduk.
        StringBuilder sb = new StringBuilder();
        //Her byte'i dizi içerisinden alarak string türüne dönüştürdük.

        foreach (byte ba in dizi)
        {
            sb.Append(ba.ToString("x2").ToLower());
        }

        //hexadecimal(onaltılık) stringi geri döndürdük.
        return sb.ToString();
    }

}
}
