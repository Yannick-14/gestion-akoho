using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Models;
using AkohoAspx.Services;
using AkohoAspx.Services.Results;

namespace AkohoAspx.Controllers
{
    public class RaceController : Controller
    {
        private readonly RaceService _raceService;

        public RaceController()
        {
            _raceService = new RaceService();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            RaceIndexData indexData = await _raceService.GetIndexDataAsync(Session["CurrentRaceId"]);
            ViewBag.CurrentRaceId = indexData.CurrentRaceId;
            return View(indexData.Races);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string nom, int jourFoyAtody)
        {
            OperationResult<int> result = await _raceService.CreateRaceAsync(nom, jourFoyAtody);
            SetRaceTempData(result);

            if (result.IsSuccess)
            {
                Session["CurrentRaceId"] = result.Value;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> createCroissanceRace(int raceId, List<CroissancePoidsRace> leftItems, List<CroissanceAlimentRace> rightItems)
        {
            OperationResult result = await _raceService.CreateCroissanceRaceAsync(
                raceId,
                Session["CurrentRaceId"],
                leftItems,
                rightItems);

            SetRaceTempData(result);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> addPrixUnitaire(string raceId, string prix)
        {
            OperationResult result = await _raceService.AddPrixUnitaireAsync(raceId, prix, Session["CurrentRaceId"]);
            SetRaceTempData(result);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetCurrentRace()
        {
            OperationResult result = _raceService.BuildResetCurrentRaceResult();
            Session.Remove("CurrentRaceId");
            SetRaceTempData(result);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _raceService.Dispose();
            }

            base.Dispose(disposing);
        }

        private void SetRaceTempData(IOperationResult result)
        {
            if (result == null || string.IsNullOrWhiteSpace(result.Message))
            {
                return;
            }

            TempData[result.IsSuccess ? "RaceSuccess" : "RaceError"] = result.Message;
        }
    }
}
