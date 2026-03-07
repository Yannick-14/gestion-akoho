using System.Collections.Generic;
using AkohoAspx.Models;

namespace AkohoAspx.Services
{
    public class SakafoIndexData
    {
        public IReadOnlyList<CroissanceAlimentRace> CroissanceAlimentRace { get; set; }
        public IReadOnlyList<Race> Race { get; set; }
        public IReadOnlyList<Lot> Lot { get; set; }
        public IReadOnlyList<CroissancePoidsRace> CroissancePoidsRace { get; set; }

        public SakafoIndexData()
        {
            CroissanceAlimentRace = new List<CroissanceAlimentRace>();
            Race = new List<Race>();
            Lot = new List<Lot>();
            CroissancePoidsRace = new List<CroissancePoidsRace>();
        }
    }
}
