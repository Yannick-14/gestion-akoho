using System.Collections.Generic;
using AkohoAspx.Models;

namespace AkohoAspx.Services
{
    public class DashBoardIndexData
    {
        public IReadOnlyList<Lot> Lots { get; set; }
        public IReadOnlyList<Race> Races { get; set; }

        public DashBoardIndexData()
        {
            Lots = new List<Lot>();
            Races = new List<Race>();
        }
    }
}
