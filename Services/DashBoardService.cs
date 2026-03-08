using System;
using System.Collections.Generic;
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
        private readonly MouvementLotRepository _mouvementLotRepository;

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
            _mouvementLotRepository = new MouvementLotRepository(_dbContext);
        }

        public async Task<DashboardLotItem> GetDashboardDataAsync()
        {
            var dateActuelle = Time.GetDateActuelle();
            var lots = await _lotRepository.GetAllAtDateAsync(dateActuelle);

            // region return des données
            var resteActuelLots = new Dictionary<int, int>();
            var prixTotalNourritureLots = new Dictionary<int, decimal>();
            var poidsFinalUnitaireLots = new Dictionary<int, int>();
            var prixVenteLots = new Dictionary<int, decimal>();
            var prixVenteRaceUnitaireLots = new Dictionary<int, decimal>();
            var semaineEcoulerLots = new Dictionary<int, int>();
            var totalMortsLots = new Dictionary<int, int>();

            // end region return des données

            foreach (var lot in lots)
            {
                semaineEcoulerLots[lot.Id] = Time.getSemaineEcouler(lot.Creation, dateActuelle);
                totalMortsLots[lot.Id] = await _mouvementLotRepository.getTotalMortDansLot(lot.Id);
                var resteNombre = await _mouvementLotRepository.resteActuelleLot(lot.Id);
                resteActuelLots[lot.Id] = resteNombre;
                prixTotalNourritureLots[lot.Id] = await GetTotalPrixNourritureParLotAsync(lot, dateActuelle);

                var poidsFinal = await GetPoidsFinalUnitaireAsync(lot, dateActuelle);
                poidsFinalUnitaireLots[lot.Id] = poidsFinal;

                // Calcul du prix de vente total estimé du lot à l'instant T
                var prixVente = await _prixVenteRaceRepository.GetLatestPrixVenteByRaceId(lot.RaceId);
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
                LotOeufsActive = await _lotOeufRepository.GetLotOeufsActive(dateActuelle),
                Lots = lots,
                ResteActuelLots = resteActuelLots,
                PrixTotalNourritureLots = prixTotalNourritureLots,
                PoidsFinalUnitaireLots = poidsFinalUnitaireLots,
                PrixVenteLots = prixVenteLots,
                PrixVenteRaceUnitaireLots = prixVenteRaceUnitaireLots,
                SemaineEcouler = semaineEcoulerLots,
                TotalMortLots = totalMortsLots
            };
        }

        private async Task<decimal> GetTotalPrixNourritureParLotAsync(Lot lot, DateTime dateActuelle)
        {
            int semaineActuelle = Time.getSemaineEcouler(lot.Creation, dateActuelle); // Utilise la nouvelle logique incluant la semaine entamée

            if (semaineActuelle <= 0) return 0;

            var restesParSemaine = await _mouvementLotRepository.getResteParSemaine(lot, dateActuelle);
            
            var croissancesAliment = await _dbContext.CroissancesAlimentRace
                .Where(c => c.RaceId == lot.RaceId)
                .OrderBy(c => c.ValueSemaine)
                .ToListAsync();

            var prixApplicable = await _prixNourritureRaceRepository.GetLatestPrixNourritureRaceId(lot.RaceId);
            if (prixApplicable == null) return 0;

            decimal prixUnitaireGramme = prixApplicable.Prix / prixApplicable.ValeurGrame;
            decimal coutTotal = 0;

            for (int s = 1; s <= semaineActuelle; s++)
            {
                int pouletsVivants = s <= restesParSemaine.Count ? restesParSemaine[s - 1] : (restesParSemaine.LastOrDefault() == 0 && restesParSemaine.Count == 0 ? lot.NombreInitial : restesParSemaine.LastOrDefault());
                
                var croissanceSemaine = croissancesAliment.FirstOrDefault(c => c.ValueSemaine == s) 
                                        ?? croissancesAliment.LastOrDefault(c => c.ValueSemaine <= s) 
                                        ?? croissancesAliment.FirstOrDefault();
                int consommationGrammes = croissanceSemaine != null ? croissanceSemaine.PoidsMoyen : 0;
                
                if (consommationGrammes == 0 || pouletsVivants == 0) continue;
                Console.WriteLine($"lot {lot.NomLot} semaine: {s} sakafo lany: {consommationGrammes} reste {pouletsVivants} prixsakafo {prixUnitaireGramme}");

                decimal coutSemaine = pouletsVivants * consommationGrammes * prixUnitaireGramme;
                coutTotal += coutSemaine;
            }


            return coutTotal;
        }

        private async Task<int> GetPoidsFinalUnitaireAsync(Lot lot, DateTime dateActuelle)
        {
            int semainesEcoulees = Time.getSemaineEcouler(lot.Creation, dateActuelle);

            // S0 = Moins de 7 jours révolus = Poids Initial seulement. 
            // Avec la nouvelle logique, on veut que le cumul ne commence qu'APRES la première semaine complète ou entamée ?
            // L'utilisateur dit : 10j = 2 semaines. Donc S0 < 1 semaine ?
            TimeSpan diff = dateActuelle - lot.Creation;
            if (diff.TotalDays < 7) return lot.PoidsInitiale;


            var croissancesPoids = await _dbContext.CroissancesPoidsRace
                .Where(c => c.RaceId == lot.RaceId)
                .OrderBy(c => c.ValueSemaine)
                .ToListAsync();

            int poidsCumule = lot.PoidsInitiale;

            // Somme cumulative des gains des semaines ÉXISTANTES dans la table (l'augmentation s'arrête à la fin de la table)
            foreach (var cp in croissancesPoids.Where(c => c.ValueSemaine <= semainesEcoulees))
            {
                poidsCumule += cp.PoidsMoyen;
            }

            return poidsCumule;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
