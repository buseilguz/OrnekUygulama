using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace OrnekUygulama.Models

{
    public partial class Kullanicilar
    {
        public Kullanicilar()
        {
            Yorumlars = new HashSet<Yorumlar>();
        }

        public int KullaniciId { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string Eposta { get; set; }
        public string Telefon { get; set; }
        public string Parola { get; set; }
        public Boolean Aktif { get; set; }
        public Boolean Silindi { get; set; }
        public Boolean Yetki { get; set; }
        public virtual ICollection<Yorumlar> Yorumlars { get; set; }
    }
}
