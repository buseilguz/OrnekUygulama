using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace OrnekUygulama.Models
{
    public partial class Yorumlar
    {
        public int YorumId { get; set; }
        public string Yorum { get; set; }
        public DateTime? Eklemetarihi { get; set; }
        public int? TarifId { get; set; }
        public int? KullaniciId { get; set; }
        public Boolean Aktif { get; set; }
        public Boolean Silindi { get; set; }

        public virtual Yemektarifleri Tarif { get; set; }
        public virtual Kullanicilar Uye { get; set; }
    }
}
