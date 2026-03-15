using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;
using AkohoAspx.Utils;
using AkohoAspx.Services.Results;

namespace AkohoAspx.Services
{
    public class AlimentService
    {
        // Calculer combien d'aliment peut on servir dans x jour ex: J1-J3, x = 3, poidsHebdo = 50g, => res = poidsHebdo / x
        public decimal getPoidsAlimentXjours(decimal poidsHebdo, int nbJourDef)
        {
            return (decimal) (poidsHebdo / 7M) * nbJourDef;
        }
    }
}
