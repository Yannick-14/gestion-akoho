using System;

namespace AkohoAspx.Utils
{
    public class Time
    {
        // ✅ Propriété avec getter/setter + valeur par défaut
        public DateTime DateActuelle { get; private set; } = DateTime.Now;

        // ✅ Setter explicite
        public void SetDateActuelle(DateTime date)
        {
            DateActuelle = date;
        }

        // ✅ Getter explicite
        public DateTime GetDateActuelle()
        {
            return DateActuelle;
        }

        public static DateTime creationDateAvecJour(int jours)
        {
            return DateTime.Now.AddDays(jours);
        }

        public static int getSemaineEcouler(DateTime date)
        {
            DateTime dateActuelle = new Time().GetDateActuelle();
            TimeSpan difference = dateActuelle - date;
            int semaines = (int)(difference.TotalDays / 7);
            return semaines < 0 ? 0 : semaines;
        }
    }
}
// EXEMPLE UTILISATION
// Time time = new Time();

// // Getter — récupère la date (DateTime.Now par défaut)
// DateTime d = time.GetDateActuelle();

// // Setter — définir une date personnalisée
// time.SetDateActuelle(new DateTime(2026, 1, 15));
// DateTime d2 = time.GetDateActuelle(); // → 15/01/2026
