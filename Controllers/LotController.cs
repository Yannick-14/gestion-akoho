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
        private readonly LotOeufService _lotOeufService;

        public LotController()
        {
            _lotService = new LotService();
            _lotOeufService = new LotOeufService();
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
        public async Task<ActionResult> Create(FormCollection requestForm)
        {
            OperationResult result = await _lotService.CreateLotAsync(requestForm);

            SetLotTempData(result);
            return RedirectToAction("Index");
        }

        // controlleur qui permet de creer un nouvel lot avec l'extraction d'oeuf vennant d'un lot
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateLotAtody(FormCollection requestForm)
        {
            OperationResult result = await _lotOeufService.CreateLotOeuf(requestForm);

            SetLotTempData(result);
            return RedirectToAction("Index", "Dashboard");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)  _lotService.Dispose();

            base.Dispose(disposing);
        }

        private void SetLotTempData(IOperationResult result)
        {
            if (result == null || string.IsNullOrWhiteSpace(result.Message)) return;
            TempData[result.IsSuccess ? "LotSuccess" : "LotError"] = result.Message;
        }
    }
}
