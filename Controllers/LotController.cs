using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Services;
using AkohoAspx.Services.Results;

namespace AkohoAspx.Controllers
{
    public class LotController : Controller
    {
        private readonly LotService _lotService;

        public LotController()
        {
            _lotService = new LotService();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            LotIndexData indexData = await _lotService.GetIndexDataAsync();
            ViewBag.Races = indexData.Races;
            return View(indexData.Lots);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            string nomLot,
            string raceId,
            string nombreInitial,
            string poidsAchat,
            string totalInvesti)
        {
            OperationResult result = await _lotService.CreateLotAsync(
                nomLot,
                raceId,
                nombreInitial,
                poidsAchat,
                totalInvesti);

            SetLotTempData(result);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _lotService.Dispose();
            }

            base.Dispose(disposing);
        }

        private void SetLotTempData(IOperationResult result)
        {
            if (result == null || string.IsNullOrWhiteSpace(result.Message))
            {
                return;
            }

            TempData[result.IsSuccess ? "LotSuccess" : "LotError"] = result.Message;
        }
    }
}
