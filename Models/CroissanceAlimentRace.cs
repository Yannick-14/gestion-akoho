namespace AkohoAspx.Models
{
    public class CroissanceAlimentRace
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public int ValueSemaine { get; set; }
        public int PoidsMoyen { get; set; }

        public virtual Race Race { get; set; }
    }
}
