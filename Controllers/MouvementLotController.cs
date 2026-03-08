using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Services;
using AkohoAspx.Services.Results;

namespace AkohoAspx.Controllers
{
    public class MouvementLotController : Controller
    {
        private readonly MouvementLotService _mouvementLotService;

        public MouvementLotController()
        {
            _mouvementLotService = new MouvementLotService();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View("RestrictionNombre");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Restriction(FormCollection requestForm)
        {
            OperationResult result = await _mouvementLotService.creationMouvement(requestForm);

            SetLotTempData(result);
            return RedirectToAction("/Dashboard/Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _mouvementLotService.Dispose();
            }

            base.Dispose(disposing);
        }

        private void SetLotTempData(IOperationResult result)
        {
            if (result == null || string.IsNullOrWhiteSpace(result.Message)) return;

            TempData[result.IsSuccess ? "LotSuccess" : "LotError"] = result.Message;
        }
    }
}
