using System.Collections.Generic;

namespace AkohoAspx.Models
{
    public class TypeMouvement
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<MouvementLot> Mouvements { get; set; }

        public TypeMouvement()
        {
            Nom = string.Empty;
            Mouvements = new List<MouvementLot>();
        }
    }
}
