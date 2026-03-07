using System;
using System.Collections.Generic;

namespace AkohoAspx.Models
{
    public class LotOeuf
    {
        public int Id { get; set; }
        public DateTime Creation { get; set; }
        public DateTime? DateEclosion { get; set; }
        public int LotParentId { get; set; }
        public int RaceId { get; set; }
        public int NbOeufs { get; set; }
        public decimal Pourcentage { get; set; }
        public bool Validation { get; set; } = false;

        public virtual Race Race { get; set; }
        public virtual Lot ParentLot { get; set; }
        public virtual Lot LotParent
        {
            get { return ParentLot; }
            set { ParentLot = value; }
        }
        public virtual ICollection<Lot> Lots { get; set; }

        public LotOeuf()
        {
            Lots = new List<Lot>();
        }
    }
}
