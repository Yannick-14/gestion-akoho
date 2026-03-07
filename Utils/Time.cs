using System;

namespace AkohoAspx.Utils
{
    public class Time
    {
        public static DateTime creationDateAvecJour(int jours)
        {
            return DateTime.Now.AddDays(jours);
        }
    }
}
