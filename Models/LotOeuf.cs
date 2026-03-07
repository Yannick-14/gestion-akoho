using System;
using System.Collections.Generic;

namespace AkohoAspx.Models
{
    public class LotOeuf
    {
        public int Id { get; set; }
        public DateTime Creation { get; set; }
        public DateTime? dateEclosion { get; set; }
        public int LotParentId { get; set; }
        public int RaceId { get; set; }
        public int nbOeufs { get; set; }
        public decimal pourcentage { get; set; }
        public bool validation { get; set; } = false;

        public virtual Race Race { get; set; }
        public virtual Lot ParentLot { get; set; }

        public LotOeuf()
        {
            ParentLot = new List<Lot>();
            Mouvements = new List<MouvementLot>();
        }
    }
}
