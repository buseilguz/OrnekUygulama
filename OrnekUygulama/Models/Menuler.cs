using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace OrnekUygulama.Models
{
    public partial class Menuler
    {
        public int MenuId { get; set; }
        public string Baslik { get; set; }
        public string Url { get; set; }
        public int? Sira { get; set; }
        public int? Ustid { get; set; }
        public Boolean Aktif { get; set; }
        public Boolean Silindi { get; set; }
    }
}
