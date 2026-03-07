using System;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;

namespace AkohoAspx.Services
{
    public class DashBoardService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly LotOeufRepository _lotOeufRepository;

        public DashBoardService()
            : this(new AppDbContext())
        {
        }

        public DashBoardService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _lotOeufRepository = new LotOeufRepository(_dbContext);
        }

        public async Task<DashboardLotItem> GetDashboardDataAsync()
        {
            var lots = await _lotRepository.GetAllAsync();
            var mouvementRepo = new MouvementLotRepository(_dbContext);
            var resteActuelLots = new System.Collections.Generic.Dictionary<int, int>();
            var prixTotalNourritureLots = new System.Collections.Generic.Dictionary<int, decimal>();
            var poidsFinalUnitaireLots = new System.Collections.Generic.Dictionary<int, int>();

            foreach (var lot in lots)
            {
                resteActuelLots[lot.Id] = await mouvementRepo.resteNombreRaceActuelleLot(lot.Id);
                prixTotalNourritureLots[lot.Id] = await GetTotalPrixNourritureParLotAsync(lot, mouvementRepo);
                poidsFinalUnitaireLots[lot.Id] = await GetPoidsFinalUnitaireAsync(lot);
            }

            return new DashboardLotItem
            {
                LotOeufsActive = await _lotOeufRepository.GetLotOeufsActive(),
                Lots = lots,
                ResteActuelLots = resteActuelLots,
                PrixTotalNourritureLots = prixTotalNourritureLots,
                PoidsFinalUnitaireLots = poidsFinalUnitaireLots
            };
        }

        private async Task<decimal> GetTotalPrixNourritureParLotAsync(Lot lot, MouvementLotRepository mouvementRepo)
        {
            int semainesEcoulees = AkohoAspx.Utils.Time.getSemaineEcouler(lot.Creation);
            if (semainesEcoulees <= 0) return 0;

            var restesParSemaine = await mouvementRepo.getResteParSemaine(lot);
            
            var croissancesAliment = await _dbContext.CroissancesAlimentRace
                .Where(c => c.RaceId == lot.RaceId && c.ValueSemaine <= semainesEcoulees)
                .ToDictionaryAsync(c => c.ValueSemaine, c => c.PoidsMoyen); // Poids en grammes consommé

            var prixNourritures = await _dbContext.PrixNourrituresRace
                .Where(p => p.RaceId == lot.RaceId)
                .OrderByDescending(p => p.Creation)
                .ToListAsync();

            if (!prixNourritures.Any()) return 0; // Pas de prix défini

            decimal coutTotal = 0;

            for (int s = 1; s <= semainesEcoulees; s++)
            {
                int pouletsVivants = s <= restesParSemaine.Count ? restesParSemaine[s - 1] : (restesParSemaine.LastOrDefault() == 0 && restesParSemaine.Count == 0 ? lot.NombreInitial : restesParSemaine.LastOrDefault());
                
                int consommationGrammes = croissancesAliment.ContainsKey(s) ? croissancesAliment[s] : 0;
                
                if (consommationGrammes == 0 || pouletsVivants == 0) continue;

                // Trouver le prix applicable pour cette semaine (le plus récent avant ou pendant la semaine)
                DateTime dateSemaine = lot.Creation.AddDays(s * 7);
                var prixApplicable = prixNourritures.FirstOrDefault(p => p.Creation <= dateSemaine) ?? prixNourritures.Last();

                decimal coutSemaine = pouletsVivants * consommationGrammes * (prixApplicable.Prix / prixApplicable.ValeurGrame);
                coutTotal += coutSemaine;
            }

            return coutTotal;
        }

        private async Task<int> GetPoidsFinalUnitaireAsync(Lot lot)
        {
            int semainesEcoulees = AkohoAspx.Utils.Time.getSemaineEcouler(lot.Creation);
            if (semainesEcoulees <= 0) return lot.PoidsInitiale;

            var croissancePoids = await _dbContext.CroissancesPoidsRace
                .Where(c => c.RaceId == lot.RaceId && c.ValueSemaine <= semainesEcoulees)
                .OrderByDescending(c => c.ValueSemaine)
                .FirstOrDefaultAsync();

            return croissancePoids != null ? croissancePoids.PoidsMoyen : lot.PoidsInitiale;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
