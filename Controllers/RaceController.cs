using System;
using System.Threading.Tasks;
using System.Web.Mvc;
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
        public async Task<ActionResult> Create(FormCollection requestForm)
        {
            OperationResult<int> result = await _raceService.CreateRaceAsync(requestForm);
            SetRaceTempData(result);

            if (result.IsSuccess)
            {
                Session["CurrentRaceId"] = result.Value;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> createCroissanceRace(FormCollection requestForm)
        {
            OperationResult result = await _raceService.CreateCroissanceRaceAsync( requestForm, Session["CurrentRaceId"]);

            SetRaceTempData(result);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> addPrixUnitaire(FormCollection requestForm)
        {
            OperationResult result = await _raceService.AddPrixUnitaireAsync(requestForm, Session["CurrentRaceId"]);
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
