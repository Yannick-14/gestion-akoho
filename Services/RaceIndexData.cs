using System.Collections.Generic;
using AkohoAspx.Models;

namespace AkohoAspx.Services
{
    public class RaceIndexData
    {
        public IReadOnlyList<Race> Races { get; set; }
        public int CurrentRaceId { get; set; }

        public RaceIndexData()
        {
            Races = new List<Race>();
        }
    }
}
