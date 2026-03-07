using System.Collections.Generic;

namespace AkohoAspx.Models
{
    public class Race
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int DureEclosionOeuf { get; set; }
        public int PoidsDefaut { get; set; }

        public virtual ICollection<CroissancePoidsRace> CroissancesPoids { get; set; }
        public virtual ICollection<CroissanceAlimentRace> CroissancesAliment { get; set; }
        public virtual ICollection<PrixVenteRace> PrixVenteRace { get; set; }
        public virtual ICollection<Lot> Lots { get; set; }

        public Race()
        {
            Nom = string.Empty;
            CroissancesPoids = new List<CroissancePoidsRace>();
            CroissancesAliment = new List<CroissanceAlimentRace>();
            PrixVenteRace = new List<PrixVenteRace>();
            Lots = new List<Lot>();
        }
    }
}
