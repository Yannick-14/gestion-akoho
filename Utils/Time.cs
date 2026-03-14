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
    }
}
