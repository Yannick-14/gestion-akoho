using System;

namespace AkohoAspx.Utils
{
    public class Time
    {
        // ✅ Getter static qui lit depuis la Session ou retourne DateTime.Now
        public static DateTime GetDateActuelle()
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["DateActuelle"] != null)
            {
                if (System.Web.HttpContext.Current.Session["DateActuelle"] is DateTime dateActuelle)
                {
                    return dateActuelle;
                }
            }
            return DateTime.Now;
        }

        // ✅ Setter static qui écrit dans la Session
        public static void SetDateActuelle(DateTime date)
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                System.Web.HttpContext.Current.Session["DateActuelle"] = date;
            }
        }

        // ✅ Reset static qui vide la Session
        public static void ResetDateActuelle()
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                System.Web.HttpContext.Current.Session.Remove("DateActuelle");
            }
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
