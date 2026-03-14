using System;

namespace AkohoAspx.Models
{
    public class PrixVenteAtody
    {
        public int Id { get; set; }
        public DateTime Creation { get; set; }
        public int RaceId { get; set; }
        public decimal Prix { get; set; } = 500;

        public virtual Race Race { get; set; }
    }
}
