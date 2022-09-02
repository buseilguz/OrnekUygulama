using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace OrnekUygulama.Models
{
    public partial class Sayfalar
    {
        public int SayfaId { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public Boolean Aktif { get; set; }
        public Boolean Silindi { get; set; }
    }
}
