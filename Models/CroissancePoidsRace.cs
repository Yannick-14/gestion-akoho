namespace AkohoAspx.Models
{
    public class CroissancePoidsRace
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public int valueSemaine { get; set; }
        public int PoidsMoyen { get; set; }

        public virtual Race Race { get; set; }

        public CroissancePoidsRace()
        {
            Semaine = string.Empty;
        }
    }
}
