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
        private readonly CroissanceRepository _croissanceRepository;
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
            _croissanceRepository = new CroissanceRepository(_dbContext);
            _prixNourritureRaceRepository = new PrixNourritureRaceRepository(_dbContext);
            _prixVenteRaceRepository = new PrixVenteRaceRepository(_dbContext);
            _mouvementLotRepository = new MouvementLotRepository(_dbContext);
        }

        public async Task<Recap> GetDashoboardData()
        {
            var dateActuelle = Time.GetDateActuelle();
            var lots = await _lotRepository.GetAllAtDateAsync(dateActuelle);

            var recap = new Recap
            {
                LotOeufsActive = (await _lotOeufRepository.GetLotOeufsActive(dateActuelle)).ToList()
            };

            foreach (var lot in lots)
            {
                var lotRecap = new LotRecap(lot)
                {
                    SemaineEcoulee = Time.getSemaineEcouler(lot.Creation, dateActuelle),
                    NombreMort = await _mouvementLotRepository.getTotalMortDansLot(lot.Id, dateActuelle)
                };

                // Calcul du nombre max de semaines de croissance pour cette race
                lotRecap.MaxWeek = await _croissanceRepository.getMaxWeek(lot.RaceId);

                lotRecap.NombreActuel = await _mouvementLotRepository.resteActuelleLot(lot.Id, dateActuelle);
                lotRecap.DepenseNourriture = await GetTotalPrixNourritureParLotAsync(lot, dateActuelle);
                lotRecap.PoidsActuelUnitaire = await GetPoidsFinalUnitaireAsync(lot, dateActuelle);

                // Calcul du prix de vente total estimé du lot à l'instant T
                var prixVente = await _prixVenteRaceRepository.GetLatestPrixVenteByRaceId(lot.RaceId);
                if (prixVente != null)
                {
                    decimal prixUnitaire = prixVente.Prix / prixVente.ValeurGrame;
                    lotRecap.PrixVenteRaceUnitaire = prixUnitaire;

                    if (lotRecap.NombreActuel > 0)
                    {
                        decimal valeurParPlume = lotRecap.PoidsActuelUnitaire * prixUnitaire;
                        lotRecap.PrixVenteLot = lotRecap.NombreActuel * valeurParPlume;
                    }
                    else { lotRecap.PrixVenteLot = 0; }
                }
                else
                {
                    lotRecap.PrixVenteRaceUnitaire = 0;
                    lotRecap.PrixVenteLot = 0;
                }

                // Calcul du Bénéfice
                lotRecap.Benefice = lotRecap.PrixVenteLot - (lotRecap.DepenseNourriture + lot.PrixAchat);

                recap.Lots.Add(lotRecap);
            }

            return recap;
        }

        private async Task<decimal> GetTotalPrixNourritureParLotAsync(Lot lot, DateTime dateActuelle)
        {
            int semaineActuelle = Time.getSemaineEcouler(lot.Creation, dateActuelle);

            if (semaineActuelle <= 0) return 0;

            // Nombre de jours réellement utilisés dans la dernière semaine (1 à 7)
            // Ex: semaine 5 commence le 29/01, dateActuelle = 31/01 → 3 jours (29, 30, 31)
            int joursEcouleesDerniereSemaine = Time.getJoursEcouleesDerniereSemaine(lot.Creation, dateActuelle);
            Console.WriteLine($"jours actif last semaine: {joursEcouleesDerniereSemaine}");

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

                decimal coutSemaine = pouletsVivants * consommationGrammes * prixUnitaireGramme;
                Console.WriteLine($"Semaine: {s}");

                // Pour la semaine courante (incomplète), on proratise selon les jours réellement utilisés
                if (s == semaineActuelle && joursEcouleesDerniereSemaine < 7)
                {
                    coutSemaine = coutSemaine * joursEcouleesDerniereSemaine / 7m;
                }

                coutTotal += coutSemaine;
            }


            return coutTotal;
        }

        private async Task<int> GetPoidsFinalUnitaireAsync(Lot lot, DateTime dateActuelle)
        {
            int semainesEcoulees = Time.getSemaineEcouler(lot.Creation, dateActuelle);

            TimeSpan diff = dateActuelle - lot.Creation;
            if (diff.TotalDays < 7) return lot.PoidsInitiale;

            var croissancesPoids = await _croissanceRepository.getCroissancePoidsRace(lot.RaceId, semainesEcoulees);
            int poidsCumule = lot.PoidsInitiale != 0 ? lot.PoidsInitiale : croissancesPoids[0].PoidsMoyen;

            foreach (var cp in croissancesPoids)
            {
                if (lot.PoidsInitiale != 0 && cp == croissancesPoids[0])
                {
                    continue;
                }
                poidsCumule += cp.PoidsMoyen;
            }

            return poidsCumule;
        }

        // calculer le total de prix de vente d'un lot par poids au poids unitaire d'une race avec reference de date actuelle
        // private async Task<decimal> GetPrixVenteTotalPoids(decimal poidsUnitaire, int raceId) {
        //     var prixVentePoids = await _prixVenteRaceRepository.GetLatestPrixVenteByRaceId(raceId);
        //     int totalAkohoActualiser = await 
        // }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
