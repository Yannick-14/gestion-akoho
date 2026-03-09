using System.Collections.Generic;
using System.Web.Mvc;
using AkohoAspx.Services;

namespace AkohoAspx.ViewsCodeBehind.Dashboard
{
    public class DashboardIndexPage : ViewPage<DashboardLotItem>
    {
        public class LotDetails
        {
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
        }

        public LotDetails GetLotDetails(AkohoAspx.Models.Lot lot)
        {
            if (Model == null) return new LotDetails();

            int id = lot.Id;
            decimal pVenteLot = Model.PrixVenteLots.ContainsKey(id) ? Model.PrixVenteLots[id] : 0;
            decimal dNourriture = Model.PrixTotalNourritureLots.ContainsKey(id) ? Model.PrixTotalNourritureLots[id] : 0;

            return new LotDetails
            {
                NombreActuel = Model.ResteActuelLots.ContainsKey(id) ? Model.ResteActuelLots[id] : lot.NombreInitial,
                NombreMort = Model.TotalMortLots.ContainsKey(id) ? Model.TotalMortLots[id] : 0,
                DepenseNourriture = dNourriture,
                PrixVenteLot = pVenteLot,
                SemaineEcoulee = Model.SemaineEcouler.ContainsKey(id) ? Model.SemaineEcouler[id] : 0,
                PoidsActuelUnitaire = Model.PoidsFinalUnitaireLots.ContainsKey(id) ? Model.PoidsFinalUnitaireLots[id] : 0,
                PrixVenteRaceUnitaire = Model.PrixVenteRaceUnitaireLots.ContainsKey(id) ? Model.PrixVenteRaceUnitaireLots[id] : 0,
                MaxWeek = Model.MaxSemaineCroissanceLots.ContainsKey(id) ? Model.MaxSemaineCroissanceLots[id] : 0,
                Benefice = pVenteLot - (dNourriture + lot.PrixAchat)
            };
        }
    }
}
