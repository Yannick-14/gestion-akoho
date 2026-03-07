using System.Collections.Generic;
using AkohoAspx.Models;

namespace AkohoAspx.Services
{
    public class LotIndexData
    {
        public IReadOnlyList<Lot> Lots { get; set; }
        public IReadOnlyList<Race> Races { get; set; }

        public LotIndexData()
        {
            Lots = new List<Lot>();
            Races = new List<Race>();
        }
    }
}
