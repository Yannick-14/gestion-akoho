using System.Collections.Generic;

namespace AkohoAspx.Models
{
    public class PrixVenteRace
    {
        public int Id { get; set; }
        public DateTime Creation { get; set; }
        public int RaceId { get; set; }
        public decimal prix { get; set; }
        public int valeurGrame { get; set; } = 1;

        public virtual Race Race { get; set; }

        public PrixVenteRace()
        {
            Race = new Race();
        }
    }
}
