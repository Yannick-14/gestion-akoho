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

        public static int getSemaineEcouler(DateTime date)
        {
            DateTime dateActuelle = GetDateActuelle();
            return getSemaineEcouler(date, dateActuelle);
        }

        public static int getSemaineEcouler(DateTime date, DateTime dateActuelle)
        {
            TimeSpan difference = dateActuelle - date;
            int semaines = (int)(difference.TotalDays / 7);
            return semaines < 0 ? 0 : semaines;
        }
    }
}
