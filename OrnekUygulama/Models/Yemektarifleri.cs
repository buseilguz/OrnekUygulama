using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace OrnekUygulama.Models
{
    public partial class Yemektarifleri
    {
        public Yemektarifleri()
        {
            Yorumlars = new HashSet<Yorumlar>();
        }

        public int TarifId { get; set; }
        public string Yemekadi { get; set; }
        public string Tarif { get; set; }
        public int? Sira { get; set; }
        public int? Kategoriid { get; set; }
        public Boolean Aktif { get; set; }
        public Boolean Silindi { get; set; }

        public virtual Kategoriler Kategori { get; set; }
        public virtual ICollection<Yorumlar> Yorumlars { get; set; }
    }
}
