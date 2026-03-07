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
        public virtual ICollection<PrixVenteRaceParPoids> PrixVentesParPoids { get; set; }
        public virtual ICollection<Lot> Lots { get; set; }

        public Race()
        {
            Nom = string.Empty;
            CroissancesPoids = new List<CroissancePoidsRace>();
            CroissancesAliment = new List<CroissanceAlimentRace>();
            PrixVentesParPoids = new List<PrixVenteRaceParPoids>();
            Lots = new List<Lot>();
        }
    }
}
