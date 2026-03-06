namespace AkohoAspx.Models
{
    public class PrixVenteRaceParPoids
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public int PoidsGrame { get; set; }
        public decimal Prix { get; set; }

        public virtual Race Race { get; set; }

        public PrixVenteRaceParPoids()
        {
            PoidsGrame = 1;
        }
    }
}
