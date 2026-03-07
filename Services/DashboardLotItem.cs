using System.Collections.Generic;
using AkohoAspx.Models;

namespace AkohoAspx.Services
{
    public class DashboardLotItem
    {
        public IReadOnlyList<LotOeuf> LotOeufsActive { get; set; }
        public IReadOnlyList<Lot> Lots { get; set; }

        public DashboardLotItem()
        {
            LotOeufsActive = new List<LotOeuf>();
            Lots = new List<Lot>();
        }
    }
}
