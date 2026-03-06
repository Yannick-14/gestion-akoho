using System;

namespace AkohoAspx.Models
{
    public class MouvementLot
    {
        public int Id { get; set; }
        public DateTime Creation { get; set; }
        public int LotId { get; set; }
        public int Quantite { get; set; }
        public int TypeId { get; set; }

        public virtual Lot Lot { get; set; }
        public virtual TypeMouvement TypeMouvement { get; set; }
    }
}
