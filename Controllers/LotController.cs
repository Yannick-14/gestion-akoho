using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;

namespace AkohoAspx.Controllers
{
    public class LotController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly RaceRepository _raceRepository;

        public LotController()
        {
            _dbContext = new AppDbContext();
            _lotRepository = new LotRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IReadOnlyList<Lot> lots = await _lotRepository.GetAllAsync();
            IReadOnlyList<Race> races = await _raceRepository.GetAllAsync();

            ViewBag.Races = races;
            return View(lots);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FormCollection form)
        {
            string nomLot = (form["nomLot"] ?? string.Empty).Trim();

            int raceId;
            int nombreInitial;
            int poidsAchat;
            decimal totalInvesti;

            bool raceOk = int.TryParse(form["raceId"], out raceId);
            bool nombreOk = int.TryParse(form["nombreInitial"], out nombreInitial);
            bool poidsOk = int.TryParse(form["poidsAchat"], out poidsAchat);

            string totalRaw = form["totalInvesti"] ?? "0";
            bool totalOk = decimal.TryParse(totalRaw, NumberStyles.Number, CultureInfo.CurrentCulture, out totalInvesti)
                           || decimal.TryParse(totalRaw, NumberStyles.Number, CultureInfo.InvariantCulture, out totalInvesti);

            if (string.IsNullOrWhiteSpace(nomLot) || !raceOk || raceId <= 0 || !nombreOk || !poidsOk || !totalOk)
            {
                TempData["LotError"] = "Donnees invalides. Verifiez les champs du formulaire.";
                return RedirectToAction("Index");
            }

            if (!await _raceRepository.ExistsAsync(raceId))
            {
                TempData["LotError"] = "Race introuvable.";
                return RedirectToAction("Index");
            }

            var lot = new Lot
            {
                NomLot = nomLot,
                RaceId = raceId,
                NombreInitial = nombreInitial,
                PoidsAchat = poidsAchat,
                TotalInvesti = totalInvesti,
                Creation = DateTime.Now,
                Statu = 0
            };

            try
            {
                await _lotRepository.creationLot(lot);
                TempData["LotSuccess"] = "Lot cree avec succes.";
            }
            catch (Exception ex)
            {
                TempData["LotError"] = "Insertion lot echouee: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
