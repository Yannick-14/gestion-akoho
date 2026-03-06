namespace AkohoAspx.Models
{
    public class CroissancePoidsRace
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public string Semaine { get; set; }
        public int Poids { get; set; }

        public virtual Race Race { get; set; }

        public CroissancePoidsRace()
        {
            Semaine = string.Empty;
        }
    }
}
