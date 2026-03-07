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
    public class DashboardLotItem
    {
        public Lot Lot { get; set; }
        public LotOeuf LotOeuf { get; set; }
        // public int ResteNombreActuel { get; set; }
        // public int Mort { get; set; }
        // public decimal Benefice { get; set; }
        // public decimal DepenseSakafo { get; set; }
    }
}
