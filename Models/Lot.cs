using System;
using System.Collections.Generic;

namespace AkohoAspx.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public DateTime Creation { get; set; }
        public string NomLot { get; set; }
        public int RaceId { get; set; }
        public int NombreInitial { get; set; }
        public int PoidsInitiale { get; set; }
        public decimal PrixAchat { get; set; }
        public int? LotOeufId { get; set; }

        public virtual Race Race { get; set; }
        public virtual Lot ParentLot { get; set; }
        public virtual LotOeuf LotOeuf { get; set; }
        public virtual TypeLot TypeLot { get; set; }

        public Lot()
        {
            NomLot = string.Empty;
            LotOeuf = new List<LotOeuf>();
        }
    }
}
