using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace OrnekUygulama.Models
{
    public partial class Kategoriler
    {
        public Kategoriler()
        {
            Yemektarifleris = new HashSet<Yemektarifleri>();
        }

        public int KategoriId { get; set; }
        public string Kategoriadi { get; set; }
        public Boolean Aktif { get; set; }
        public Boolean Silindi { get; set; }

        public virtual ICollection<Yemektarifleri> Yemektarifleris { get; set; }
    }
}
