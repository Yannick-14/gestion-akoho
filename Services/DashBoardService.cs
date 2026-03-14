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
        private readonly AlimentService alimentService;

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
            alimentService = new AlimentService();
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
                lotRecap.DepenseNourriture = await depenseAlimentaireTotale(lot, dateActuelle);
                lotRecap.PoidsActuelUnitaire = await GetPoidsFinalUnitaireAsync(lot, dateActuelle);
                lotRecap.PrixVenteLot = await GetPrixVenteTotalPoids(lotRecap.PoidsActuelUnitaire, dateActuelle, lot);
                // Calcul du prix de vente total estimé du lot à l'instant T
                var prixVente = await _prixVenteRaceRepository.GetLatestPrixVenteByRaceId(lot.RaceId);
                lotRecap.PrixVenteRaceUnitaire = prixVente.Prix / prixVente.ValeurGrame;
                // if (prixVente != null)
                // {
                //     decimal prixUnitaire = prixVente.Prix / prixVente.ValeurGrame;
                //     lotRecap.PrixVenteRaceUnitaire = prixUnitaire;

                //     if (lotRecap.NombreActuel > 0)
                //     {
                //         decimal valeurParPlume = lotRecap.PoidsActuelUnitaire * prixUnitaire;
                //         lotRecap.PrixVenteLot = lotRecap.NombreActuel * valeurParPlume;
                //     }
                //     else { lotRecap.PrixVenteLot = 0; }
                // }
                // else
                // {
                //     lotRecap.PrixVenteRaceUnitaire = 0;
                //     lotRecap.PrixVenteLot = 0;
                // }

                // Calcul du Bénéfice
                lotRecap.Benefice = lotRecap.PrixVenteLot - (lotRecap.DepenseNourriture + lot.PrixAchat);

                recap.Lots.Add(lotRecap);
            }

            return recap;
        }

        private async Task<decimal> getSakafoGrammeDepenser(Lot lot, DateTime dateActuelle) {
            var fluxJournalier = await _mouvementLotRepository.getFluxJournalierParSemaine(lot, dateActuelle);
            
            var croissancesAliment = await _croissanceRepository.getCroissanceAlimentRace(lot.RaceId);

            Console.WriteLine($"\n--- FLUX JOURNALIER LOT {lot.NomLot} ---");
            decimal totalPoidsActualiser = 0.0M;
            for (int i = 0; i < fluxJournalier.Count; i++)
            {
                var jours = fluxJournalier[i];
                int s = i + 1; // Numéro de semaine pour l'affichage
                
                var croissanceSemaine = croissancesAliment.FirstOrDefault(c => c.ValueSemaine == s)
                                        ?? croissancesAliment.LastOrDefault(c => c.ValueSemaine <= s)
                                        ?? croissancesAliment.FirstOrDefault();

                decimal poidsHebdo = croissanceSemaine != null ? croissanceSemaine.PoidsMoyen : 0;
                // decimal poidsJournalier = poidsHebdo / 7m;

                // Console.WriteLine($"s {s} - jours count {jours.Count} | Poids Hebdo: {poidsHebdo}g | Poids/Jour: {poidsJournalier:F2}g");
                
                Console.Write($"S{s}: ");
                int jourIdx = 0;
                while (jourIdx < jours.Count)
                {
                    int valeur = jours[jourIdx];
                    int debut  = jourIdx + 1;
                    while (jourIdx < jours.Count && jours[jourIdx] == valeur) jourIdx++;
                    int fin = jourIdx;
                    int countJours = fin - debut + 1;

                    // decimal poidsPeriode = countJours * poidsJournalier;
                    decimal poidsPeriode = alimentService.getPoidsAlimentXjours(poidsHebdo, countJours);
                    totalPoidsActualiser += poidsPeriode * valeur;

                    if (debut == fin)
                        Console.Write($"J{debut} = {valeur} (x{countJours}j = {poidsPeriode:F2}g)");
                    else
                        Console.Write($"J{debut}-J{fin} = {valeur} (x{countJours}j = {poidsPeriode:F2}g)");

                    if (jourIdx < jours.Count) Console.Write(", ");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"total= {totalPoidsActualiser}--- FIN ---\n");
            return totalPoidsActualiser;
        }

        private async Task<decimal> depenseAlimentaireTotale(Lot lot, DateTime dateActuelle) {
            decimal poidsTotal = await getSakafoGrammeDepenser(lot, dateActuelle);
            var prixGramme = await _prixNourritureRaceRepository.GetLatestPrixNourritureRaceId(lot.RaceId);
            return poidsTotal * prixGramme.Prix;
        }

        private async Task<int> GetPoidsFinalUnitaireAsync(Lot lot, DateTime dateActuelle)
        {
            int semainesEcoulees = Time.getSemaineEcouler(lot.Creation, dateActuelle);

            TimeSpan diff = dateActuelle - lot.Creation;
            if (diff.TotalDays < 7) return lot.PoidsInitiale;

            var croissancesPoids = await _croissanceRepository.getCroissancePoidsRace(lot.RaceId, semainesEcoulees);
            if (croissancesPoids == null || croissancesPoids.Count == 0) return lot.PoidsInitiale;

            int poidsCumule = lot.PoidsInitiale != 0 ? lot.PoidsInitiale : croissancesPoids[0].PoidsMoyen;
            int joursDerniereSemaine = Time.getJoursEcouleesDerniereSemaine(lot.Creation, dateActuelle);
            // Console.WriteLine($"joursDerniereSemaine: {joursDerniereSemaine}");

            foreach (var cp in croissancesPoids)
            {
                if (lot.PoidsInitiale != 0 && cp == croissancesPoids[0])
                {
                    continue;
                }

                if (cp.ValueSemaine == semainesEcoulees && joursDerniereSemaine < 7)
                {
                    double poidsJournalier = cp.PoidsMoyen / 7.0;
                    poidsCumule += (int)Math.Round(poidsJournalier * joursDerniereSemaine);
                }
                else
                {
                    poidsCumule += cp.PoidsMoyen;
                }
            }

            return poidsCumule;
        }

        // calculer le total de prix de vente d'un lot par poids au poids unitaire d'une race avec reference de date actuelle
        private async Task<decimal> GetPrixVenteTotalPoids(decimal poidsUnitaire, DateTime dateActuelle, Lot lot) {
            var prixVentePoids = await _prixVenteRaceRepository.GetLatestPrixVenteByRaceId(lot.RaceId);
            int totalAkohoActualiser = await _mouvementLotRepository.resteActuelleLot(lot.Id, dateActuelle);
            return  ((decimal)totalAkohoActualiser * poidsUnitaire) * prixVentePoids.Prix;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
