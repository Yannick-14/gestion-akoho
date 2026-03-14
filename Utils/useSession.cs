using System;
using System.Web;

namespace AkohoAspx.Utils
{
    public class useSession
    {
        private static HttpContext Current => HttpContext.Current;

        private void setSessionDate(DateTime date)
        {
            if (Current != null && Current.Session != null)
            {
                Current.Session["DateActuelle"] = date;
            }
        }

        private void getSessionDate()
        {
            if (Current != null && Current.Session != null && Current.Session["DateActuelle"] != null)
            {
                if (Current.Session["DateActuelle"] is DateTime dateActuelle)
                {
                    return dateActuelle;
                }
            }
        }
        public static void removeSessionDate()
        {
            if (Current != null && Current.Session != null) { 
                Current.Session.Remove("DateActuelle");
            }
        }
    }
}
