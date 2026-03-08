using System;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;
using AkohoAspx.Utils;

namespace AkohoAspx.Services
{
    public class DashboardService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly LotOeufRepository _lotOeufRepository;
        private readonly PrixNourritureRaceRepository _prixNourritureRaceRepository;
        private readonly PrixVenteRaceRepository _prixVenteRaceRepository;

        public DashboardService()
            : this(new AppDbContext())
        {
        }

        public DashboardService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _lotOeufRepository = new LotOeufRepository(_dbContext);
            _prixNourritureRaceRepository = new PrixNourritureRaceRepository(_dbContext);
            _prixVenteRaceRepository = new PrixVenteRaceRepository(_dbContext);
        }

        public async Task<DashboardLotItem> GetDashboardDataAsync()
        {
            var lots = await _lotRepository.GetAllAsync();
            var mouvementRepo = new MouvementLotRepository(_dbContext);
            var resteActuelLots = new System.Collections.Generic.Dictionary<int, int>();
            var prixTotalNourritureLots = new System.Collections.Generic.Dictionary<int, decimal>();
            var poidsFinalUnitaireLots = new System.Collections.Generic.Dictionary<int, int>();
            var prixVenteLots = new System.Collections.Generic.Dictionary<int, decimal>();
            var prixVenteRaceUnitaireLots = new System.Collections.Generic.Dictionary<int, decimal>();
            var dateActuelle = new Time().GetDateActuelle();

            foreach (var lot in lots)
            {
                var resteNombre = await mouvementRepo.resteNombreRaceActuelleLot(lot.Id);
                resteActuelLots[lot.Id] = resteNombre;
                prixTotalNourritureLots[lot.Id] = await GetTotalPrixNourritureParLotAsync(lot, mouvementRepo, dateActuelle);
                
                var poidsFinal = await GetPoidsFinalUnitaireAsync(lot);
                poidsFinalUnitaireLots[lot.Id] = poidsFinal;

                // Calcul du prix de vente total estimé du lot à l'instant T
                var prixVente = await _prixVenteRaceRepository.GetLatestPrixVenteByRaceId(lot.RaceId, dateActuelle);
                if (prixVente != null)
                {
                    decimal prixUnitaire = prixVente.Prix / prixVente.ValeurGrame;
                    prixVenteRaceUnitaireLots[lot.Id] = prixUnitaire;

                    if (resteNombre > 0)
                    {
                        decimal valeurParPlume = poidsFinal * prixUnitaire;
                        prixVenteLots[lot.Id] = resteNombre * valeurParPlume;
                    }
                    else
                    {
                        prixVenteLots[lot.Id] = 0;
                    }
                }
                else
                {
                    prixVenteRaceUnitaireLots[lot.Id] = 0;
                    prixVenteLots[lot.Id] = 0;
                }
            }

            return new DashboardLotItem
            {
                LotOeufsActive = await _lotOeufRepository.GetLotOeufsActive(),
                Lots = lots,
                ResteActuelLots = resteActuelLots,
                PrixTotalNourritureLots = prixTotalNourritureLots,
                PoidsFinalUnitaireLots = poidsFinalUnitaireLots,
                PrixVenteLots = prixVenteLots,
                PrixVenteRaceUnitaireLots = prixVenteRaceUnitaireLots
            };
        }

        private async Task<decimal> GetTotalPrixNourritureParLotAsync(Lot lot, MouvementLotRepository mouvementRepo, System.DateTime dateActuelle)
        {
            int semainesEcoulees = AkohoAspx.Utils.Time.getSemaineEcouler(lot.Creation);
            if (semainesEcoulees <= 0) return 0;

            var restesParSemaine = await mouvementRepo.getResteParSemaine(lot);
            
            var croissancesAliment = await _dbContext.CroissancesAlimentRace
                .Where(c => c.RaceId == lot.RaceId && c.ValueSemaine <= semainesEcoulees)
                .ToDictionaryAsync(c => c.ValueSemaine, c => c.PoidsMoyen); // Poids en grammes consommé

            var prixApplicable = await _prixNourritureRaceRepository.GetLatestPrixNourritureRaceId(lot.RaceId, dateActuelle);
            if (prixApplicable == null) return 0; // Pas de prix défini

            decimal prixUnitaireGramme = prixApplicable.Prix / prixApplicable.ValeurGrame;
            decimal coutTotal = 0;

            for (int s = 1; s <= semainesEcoulees; s++)
            {
                int pouletsVivants = s <= restesParSemaine.Count ? restesParSemaine[s - 1] : (restesParSemaine.LastOrDefault() == 0 && restesParSemaine.Count == 0 ? lot.NombreInitial : restesParSemaine.LastOrDefault());
                
                int consommationGrammes = croissancesAliment.ContainsKey(s) ? croissancesAliment[s] : 0;
                
                if (consommationGrammes == 0 || pouletsVivants == 0) continue;

                decimal coutSemaine = pouletsVivants * consommationGrammes * prixUnitaireGramme;
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
