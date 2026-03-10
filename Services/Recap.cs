using System;
using System.Collections.Generic;
using AkohoAspx.Models;

namespace AkohoAspx.Services
{
    public class Recap
    {
        public List<LotOeuf> LotOeufsActive { get; set; } = new List<LotOeuf>();
        public List<LotRecap> Lots { get; set; } = new List<LotRecap>();
    }
}
