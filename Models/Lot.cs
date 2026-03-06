using System;
using System.Collections.Generic;

namespace AkohoAspx.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public DateTime Creation { get; set; }
        public DateTime? DateAfoyAkoho { get; set; }
        public string NomLot { get; set; }
        public int RaceId { get; set; }
        public int NombreInitial { get; set; }
        public int PoidsAchat { get; set; }
        public decimal TotalInvesti { get; set; }
        public int? LotParent { get; set; }
        public int Statu { get; set; }

        public virtual Race Race { get; set; }
        public virtual Lot ParentLot { get; set; }
        public virtual ICollection<Lot> ChildLots { get; set; }
        public virtual ICollection<MouvementLot> Mouvements { get; set; }

        public Lot()
        {
            NomLot = string.Empty;
            ChildLots = new List<Lot>();
            Mouvements = new List<MouvementLot>();
        }
    }
}
