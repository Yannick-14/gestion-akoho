using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Models;
using AkohoAspx.Services;
using AkohoAspx.Services.Results;

namespace AkohoAspx.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardService _dashboardService;
        public DashboardController()
        {
            _dashboardService = new DashboardService();
        }
        public async Task<ActionResult> Index()
        {
            var data = await _dashboardService.GetDashoboardData();
            return View(data);
        }

        public async Task<ActionResult> LotOeufs()
        {
            var data = await _dashboardService.GetDashoboardData();
            return View(data);
        }

        public ActionResult MakaAtody(int lotId, int raceId)
        {
            ViewBag.LotId = lotId;
            ViewBag.RaceId = raceId;
            return View();
        }

        [HttpGet]
        public ActionResult SignalerMaty(int lotId)
        {
            ViewBag.LotId = lotId;
            return View("RestrictionNombre");
        }

        [HttpGet]
        public ActionResult EclosOeuf(int lotOeufId)
        {
            ViewBag.LotOeufId = lotOeufId;
            return View("EclosionAtody");
        }

        [HttpPost]
        public ActionResult ActualiserDate(DateTime? DateActualiser)
        {
            if (DateActualiser.HasValue)
            {
                var dateFinDeJour = DateActualiser.Value.Date.AddDays(1).AddTicks(-1);
                AkohoAspx.Utils.Time.SetDateActuelle(dateFinDeJour);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ResetDate()
        {
            AkohoAspx.Utils.Time.ResetDateActuelle();
            return RedirectToAction("Index");
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = string.Empty });
        }
    }
}
