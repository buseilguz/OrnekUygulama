using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrnekUygulama.Models
{
    public class TarifYorumlar
    {
        public Yemektarifleri tarif { get; set; }
        public List<Yorumlar> yorumlar { get; set; }
    }
}
