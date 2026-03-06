namespace AkohoAspx.Models
{
    public class CroissanceAlimentRace
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public string Semaine { get; set; }
        public int Aliment { get; set; }

        public virtual Race Race { get; set; }

        public CroissanceAlimentRace()
        {
            Semaine = string.Empty;
        }
    }
}
