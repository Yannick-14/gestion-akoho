using System;
using System.Collections.Generic;

namespace AkohoAspx.Models
{
    public class LotRecap
    {
        public int Id { get; set; }
        public string NomLot { get; set; }
        public int RaceId { get; set; }
        public Race Race { get; set; }
        public int NombreInitial { get; set; }
        public int PoidsInitiale { get; set; }
        public decimal PrixAchat { get; set; }
        public DateTime Creation { get; set; }

        // Calculs
        public int NombreActuel { get; set; }
        public int NombreMort { get; set; }
        public decimal DepenseNourriture { get; set; }
        public decimal PrixVenteLot { get; set; }
        public int SemaineEcoulee { get; set; }
        public int PoidsActuelUnitaire { get; set; }
        public decimal PrixVenteRaceUnitaire { get; set; }
        public decimal Benefice { get; set; }
        public int MaxWeek { get; set; }
        public bool IsReadyToSell => SemaineEcoulee >= MaxWeek && MaxWeek > 0;
        public decimal PrixUnitEstime => (decimal)PoidsActuelUnitaire * PrixVenteRaceUnitaire;

        public LotRecap(Lot lot)
        {
            Id = lot.Id;
            NomLot = lot.NomLot;
            RaceId = lot.RaceId;
            Race = lot.Race;
            NombreInitial = lot.NombreInitial;
            PoidsInitiale = lot.PoidsInitiale;
            PrixAchat = lot.PrixAchat;
            Creation = lot.Creation;
        }
    }
}
