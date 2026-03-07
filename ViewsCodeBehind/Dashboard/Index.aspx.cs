using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using AkohoAspx.Models;

namespace AkohoAspx.ViewsCodeBehind.Dashboard
{
    public class DashboardIndexPage : ViewPage<dynamic>
    {
        private static readonly IReadOnlyList<DashboardLotData> StaticLots = new List<DashboardLotData>
        {
            new DashboardLotData
            {
                NomLot = "Lot A1",
                NombreContenu = 500,
                MonnaieInvesti = 1800000m,
                ResteNombreActuel = 472,
                Mort = 28,
                Benefice = 420000m
            },
            new DashboardLotData
            {
                NomLot = "Lot B2",
                NombreContenu = 650,
                MonnaieInvesti = 2340000m,
                ResteNombreActuel = 621,
                Mort = 29,
                Benefice = 510000m
            },
            new DashboardLotData
            {
                NomLot = "Lot C3",
                NombreContenu = 420,
                MonnaieInvesti = 1510000m,
                ResteNombreActuel = 398,
                Mort = 22,
                Benefice = 305000m
            },
            new DashboardLotData
            {
                NomLot = "Lot D4",
                NombreContenu = 560,
                MonnaieInvesti = 2010000m,
                ResteNombreActuel = 533,
                Mort = 27,
                Benefice = 448000m
            }
        };

        public IEnumerable<DashboardLotData> Lots
        {
            get { return StaticLots ?? Enumerable.Empty<DashboardLotData>(); }
        }
    }
}
