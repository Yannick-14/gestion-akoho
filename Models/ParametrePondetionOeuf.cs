using System;
using System.Collections.Generic;

namespace AkohoAspx.Models
{
    public class ParametrePondetionOeuf
    {
        public int Id { get; set; }
        public int LotOeufId { get; set; }
        public int PourcentageMal { get; set; }
        public int PourcentageFemelle { get; set; }

        public virtual LotOeuf LotOeuf { get; set; }
    }
}
