using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;

namespace AkohoAspx.Controllers
{
    public class RaceController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly RaceRepository _raceRepository;
        private readonly CroissanceRepository _croissanceRepository;

        public RaceController()
        {
            _dbContext = new AppDbContext();
            _raceRepository = new RaceRepository(_dbContext);
            _croissanceRepository = new CroissanceRepository(_dbContext);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IReadOnlyList<Race> races = await _raceRepository.GetAllAsync();
            ViewBag.CurrentRaceId = GetCurrentRaceIdFromSession();
            return View(races);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string nom, int jourFoyAtody)
        {
            if (string.IsNullOrWhiteSpace(nom) || jourFoyAtody <= 0)
            {
                TempData["RaceError"] = "Le nom et le jour de foy atody sont obligatoires.";
                return RedirectToAction("Index");
            }

            var race = new Race
            {
                Nom = nom.Trim(),
                JourFoyAtody = jourFoyAtody
            };

            int createdRaceId = await _raceRepository.CreateAsync(race);
            Console.WriteLine("createdRaceId: " + createdRaceId);
            Session["CurrentRaceId"] = createdRaceId;
            TempData["RaceSuccess"] = "Race creee. RaceId actif: " + createdRaceId;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> createCroissanceRace(int raceId, List<CroissancePoidsRace> leftItems, List<CroissanceAlimentRace> rightItems)
        {
            if (raceId <= 0)
            {
                raceId = GetCurrentRaceIdFromSession();
            }

            if (raceId <= 0)
            {
                TempData["RaceError"] = "Aucune race active dans la session.";
                return RedirectToAction("Index");
            }

            List<CroissancePoidsRace> left = leftItems ?? new List<CroissancePoidsRace>();
            List<CroissanceAlimentRace> right = rightItems ?? new List<CroissanceAlimentRace>();

            await _croissanceRepository.createCroissancePoidsAndAliment(raceId, left, right);
            TempData["RaceSuccess"] = (left.Count + right.Count) + " ligne(s) inseree(s) pour la race " + raceId + ".";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addPrixUnitaire(int raceId, double prix)
        {
            if (raceId <= 0)
            {
                raceId = GetCurrentRaceIdFromSession();
            }

            if (raceId <= 0)
            {
                TempData["RaceError"] = "Aucune race active dans la session.";
                return RedirectToAction("Index");
            }

            TempData["RaceSuccess"] = "Prix enregistre pour la race " + raceId + ": " + prix;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetCurrentRace()
        {
            Session.Remove("CurrentRaceId");
            TempData["RaceSuccess"] = "Race active retiree de la session.";
            return RedirectToAction("Index");
        }

        private int GetCurrentRaceIdFromSession()
        {
            object value = Session["CurrentRaceId"];
            if (value == null)
            {
                return 0;
            }

            int raceId;
            return int.TryParse(value.ToString(), out raceId) ? raceId : 0;
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
