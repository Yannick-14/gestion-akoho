using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;
using AkohoAspx.Services.Results;
using AkohoAspx.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AkohoAspx.Services
{
    public class SakafoService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly RaceRepository _raceRepository;
        private readonly MouvementLotRepository _mouvementLotRepository;
        private readonly CroissanceRepository _croissanceRepository;
        private readonly TypeMouvementRepository _typeMouvementRepository;

        public SakafoService() : this(new AppDbContext()) {}

        public SakafoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
            _mouvementLotRepository = new MouvementLotRepository(_dbContext);
            _croissanceRepository = new CroissanceRepository(_dbContext);
            _typeMouvementRepository = new TypeMouvementRepository(_dbContext);
        }

        // compter combien se sont ecoules entre la date actuelle et la date de creation de lot
        public async Task<int> compterSemaineEcouler(int lotId)
        {
            Lot lot = await _lotRepository.getInfoIntialeLot(lotId);
            if (lot == null) return 0;
            return Time.getSemaineEcouler(lot.Creation);
        }

        public async Task<IReadOnlyList<CroissanceAlimentRace>> getCroissanceRace(int lotId)
        {
            Lot lot = await _lotRepository.getInfoIntialeLot(lotId);
            if (lot == null) return new List<CroissanceAlimentRace>();

            return await _croissanceRepository.getCroissanceAlimentRace(lot.RaceId);
        }

        // Recuperer la totalite de nombre restant dans un lot a chaque semaine jusqu'a la semaine actuelle
        public async Task<List<int>> getResteParSemaine(int lotId)
        {
            Lot lot = await _lotRepository.getInfoIntialeLot(lotId);
            if (lot == null) return new List<int>();
            
            var resteParSemaine = await _mouvementLotRepository.getResteParSemaine(lot);

            for (int s = 0; s < resteParSemaine.Count; s++)
            {
                Console.WriteLine($"Semaine: {s + 1}, Reste: {resteParSemaine[s]}");
            }

            return resteParSemaine;
        }

        public async Task<int> getDepenseAlimentActuelleLotEnGramme(int lotId)
        {
            int semainesEcoulees = await compterSemaineEcouler(lotId);
            var croissanceParSemaine = await getCroissanceRace(lotId);
            Console.WriteLine($"croissanceParSemaine: {croissanceParSemaine}");
            var resteParSemaine = await getResteParSemaine(lotId);

            var alimentParSemaine = new Dictionary<int, int>();
            foreach (var c in croissanceParSemaine)
            {
                int semaine;
                string digits = new string(c.Semaine.Where(char.IsDigit).ToArray());
                if (int.TryParse(digits, out semaine) && semaine > 0)
                {
                    alimentParSemaine[semaine] = c.Aliment;
                }
            }

            int limite = Math.Min(semainesEcoulees, resteParSemaine.Count);
            int totalAliment = 0;

            for (int s = 1; s <= limite; s++)
            {
                int alimentUnitaire = alimentParSemaine.ContainsKey(s) ? alimentParSemaine[s] : 0;
                int nombreRestant = resteParSemaine[s - 1];

                int totalSemaine = alimentUnitaire * nombreRestant;
                totalAliment = totalAliment + totalSemaine;
            }

            return totalAliment;
        }


        public void Dispose() { _dbContext.Dispose(); }
    }
}
