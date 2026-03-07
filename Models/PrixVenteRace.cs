using System;

namespace AkohoAspx.Models
{
    public class PrixVenteRace
    {
        public int Id { get; set; }
        public DateTime Creation { get; set; }
        public int RaceId { get; set; }
        public decimal Prix { get; set; }
        public int ValeurGrame { get; set; } = 1;

        public virtual Race Race { get; set; }
    }
}
