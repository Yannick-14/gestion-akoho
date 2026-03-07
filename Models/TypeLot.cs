using System.Collections.Generic;

namespace AkohoAspx.Models
{
    public class TypeLot
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public TypeMouvement()
        {
            Nom = string.Empty;
        }
    }
}
