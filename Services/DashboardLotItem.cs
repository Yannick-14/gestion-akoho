using System.Collections.Generic;
using AkohoAspx.Models;

namespace AkohoAspx.Services
{
    public class DashboardLotItem
    {
        public IReadOnlyList<LotOeuf> LotOeufsActive { get; set; }
        public IReadOnlyList<Lot> Lots { get; set; }
        public Dictionary<int, int> ResteActuelLots { get; set; }
        public Dictionary<int, decimal> PrixTotalNourritureLots { get; set; }
        public Dictionary<int, int> PoidsFinalUnitaireLots { get; set; }

        public DashboardLotItem()
        {
            LotOeufsActive = new List<LotOeuf>();
            Lots = new List<Lot>();
            ResteActuelLots = new Dictionary<int, int>();
            PrixTotalNourritureLots = new Dictionary<int, decimal>();
            PoidsFinalUnitaireLots = new Dictionary<int, int>();
        }
    }
}
