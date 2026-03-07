namespace AkohoAspx.Models
{
    public class CroissanceAlimentRace
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public int valueSemaine { get; set; }
        public int PoidsMoyen { get; set; }

        public virtual Race Race { get; set; }

        public CroissanceAlimentRace()
        {
            valueSemaine = string.Empty;
        }
    }
}
