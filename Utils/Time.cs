using System;
using System.Web;

namespace AkohoAspx.Utils
{
    public class Time
    {
        private static HttpContext Current => HttpContext.Current;
        // Getter qui lit depuis la Session ou retourne DateTime.Now
        public static DateTime GetDateActuelle()
        {
            if (Current != null && Current.Session != null && Current.Session["DateActuelle"] != null)
            {
                if (Current.Session["DateActuelle"] is DateTime dateActuelle)
                {
                    return dateActuelle;
                }
            }
            return DateTime.Now;
        }

        public static void SetDateActuelle(DateTime date)
        {
            if (Current != null && Current.Session != null)
            {
                Current.Session["DateActuelle"] = date;
            }
        }

        public static void ResetDateActuelle()
        {
            if (Current != null && Current.Session != null) { Current.Session.Remove("DateActuelle"); }
        }

        public static DateTime creationDateAvecJour(int jours)
        {
            return DateTime.Now.AddDays(jours);
        }

        public static int getSemaineEcouler(DateTime date, DateTime dateActuelle)
        {
            TimeSpan difference = dateActuelle - date;
            double totalDays = difference.TotalDays;
            
            if (totalDays < 0) return 0;

            // On arrondit au supérieur pour inclure la semaine entamée (ex: 10 jours = 2 semaines)
            return (int)Math.Ceiling(totalDays / 7.0);
        }

        /// <summary>
        /// Retourne le nombre de jours réellement utilisés dans la semaine courante (entre 1 et 7).
        /// Ex: lot créé le 01/01, dateActuelle = 31/01 → semaine 5 commence le 29/01,
        ///     jours utilisés = 3 (29, 30, 31).
        /// Si la semaine est entièrement écoulée, retourne 7.
        /// </summary>
        public static int getJoursEcouleesDerniereSemaine(DateTime dateCreation, DateTime dateActuelle)
        {
            double totalDays = (dateActuelle - dateCreation).TotalDays;
            if (totalDays <= 0) return 1;

            // Combien de jours ont été écoulés dans la semaine courante (1 à 7)
            double joursRestants = totalDays % 7;

            // Si totalDays est un multiple exact de 7, la semaine est complète → 7 jours
            if (joursRestants == 0) return 7;

            // Sinon : nombre de jours entamés dans la dernière semaine (au moins 1)
            return (int)Math.Ceiling(joursRestants);
        }
    }
}
