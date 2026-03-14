using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Utils;

namespace AkohoAspx.Repository
{
    public class MouvementLotRepository
    {
        private readonly AppDbContext _dbContext;

        public MouvementLotRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MouvementLot> creationMouvement(MouvementLot transaction)
        {
            _dbContext.MouvementsLot.Add(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction;
        }

        public async Task<IReadOnlyList<MouvementLot>> findAllTransactionLot(int lotId)
        {
            return await _dbContext.MouvementsLot
                .Where(mvt => mvt.LotId == lotId)
                .ToListAsync();
        }

        //  recuperere la totalite de mort dans un lot par rapport à la date actuelle
        public async Task<int> getTotalMortDansLot(int lotId, DateTime? dateActuelle = null)
        {
            var query = _dbContext.MouvementsLot.Where(mvt => mvt.LotId == lotId);
            if (dateActuelle.HasValue) query = query.Where(mvt => mvt.Creation <= dateActuelle.Value);

            var mvts = await query.ToListAsync();
            int total = 0;
            foreach (var m in mvts)
            {
                total += m.Nombre;
            }
            return total;
        }

        // Recuperer juste le nombre restant dans un lot à cet instant
        public async Task<int> resteActuelleLot(int lotId, DateTime? dateActuelle  = null)
        {
            var lot = await _dbContext.Lots.FindAsync(lotId);
            if (lot == null) return 0;

            DateTime dateRef = dateActuelle ?? Time.GetDateActuelle();

            int totalPerteActualiser = await _dbContext.MouvementsLot
                .Where(mvt => mvt.LotId == lotId && mvt.Creation <= dateRef)
                .Select(mvt => (int?)mvt.Nombre)
                .SumAsync() ?? 0;

            return lot.NombreInitial - totalPerteActualiser;
        }

        // Recuperer le reste de nombre restant à chaque semaine jusqu' à l'actuelle par rappport à la création du lot
        public async Task<List<int>> getResteParSemaine(Lot lot, DateTime? dateActuelle = null)
        {
            var resteParSemaine = new List<int>();
            if (lot == null) return resteParSemaine;

            var dateReference = dateActuelle ?? Time.GetDateActuelle();
            int semainesEcoulees = Time.getSemaineEcouler(lot.Creation, dateReference);
            if (semainesEcoulees <= 0) return resteParSemaine;

            var query = _dbContext.MouvementsLot.Where(mvt => mvt.LotId == lot.Id && mvt.Creation >= lot.Creation);
            if (dateActuelle.HasValue) query = query.Where(mvt => mvt.Creation <= dateActuelle.Value);

            var mortsParSemaine = await query
                .GroupBy(mvt => DbFunctions.DiffDays(lot.Creation, mvt.Creation) / 7 + 1)
                .Select(g => new
                {
                    Semaine = g.Key,
                    Mort = g.Sum(mvt => (int?)mvt.Nombre) ?? 0
                })
                .ToListAsync();

            int resteCourant = lot.NombreInitial;
            if (resteCourant < 0) resteCourant = 0;

            for (int s = 1; s <= semainesEcoulees; s++)
            {
                // On ajoute d'abord le reste (celui du début de semaine)
                resteParSemaine.Add(resteCourant);

                // Puis on prépare le reste pour la semaine SUIVANTE
                var mortSemaine = mortsParSemaine.FirstOrDefault(v => v.Semaine == s);
                if (mortSemaine != null) resteCourant -= mortSemaine.Mort;
                
                if (resteCourant < 0) resteCourant = 0;
            }

            return resteParSemaine;
        }

        /// <summary>
        /// Retourne le flux journalier du reste pour chaque semaine.
        /// Chaque élément de la liste = une semaine, sous forme d'une liste de restes journaliers (7 valeurs max).
        ///
        /// Exemple (NombreInitial=50, perte de 2 au J4 et perte de 2 au J7) :
        ///   S1 → [50, 50, 50, 48, 48, 48, 46]
        ///   S2 → [46, 46, 46, ...]   (commence au reste du dernier jour de S1)
        ///
        /// La semaine courante (incomplète) ne contient que les jours réellement écoulés.
        /// </summary>
        public async Task<List<List<int>>> getFluxJournalierParSemaine(Lot lot, DateTime? dateActuelle = null)
        {
            var result = new List<List<int>>();
            if (lot == null) return result;

            var dateReference = dateActuelle ?? Time.GetDateActuelle();
            int semainesEcoulees = Time.getSemaineEcouler(lot.Creation, dateReference);
            if (semainesEcoulees <= 0) return result;

            // Chargement en mémoire de tous les mouvements du lot (tri par date)
            var mouvements = await _dbContext.MouvementsLot
                .Where(mvt => mvt.LotId == lot.Id
                           && mvt.Creation >= lot.Creation
                           && mvt.Creation <= dateReference)
                .OrderBy(mvt => mvt.Creation)
                .ToListAsync();

            // Date de création normalisée (sans heure) comme référence du calendrier
            DateTime dateCreationNorm = lot.Creation.Date;
            DateTime dateReferenceNorm = dateReference.Date;

            int resteActuel = Math.Max(0, lot.NombreInitial);

            for (int s = 1; s <= semainesEcoulees; s++)
            {
                DateTime debutSemaine = dateCreationNorm.AddDays((s - 1) * 7);
                DateTime finSemaine   = dateCreationNorm.AddDays(s * 7 - 1);

                // Pour la dernière semaine on s'arrête à dateActuelle
                if (finSemaine > dateReferenceNorm)
                    finSemaine = dateReferenceNorm;

                int nbJours = (int)(finSemaine - debutSemaine).TotalDays + 1;
                var joursResteSemaine = new List<int>(nbJours);

                for (int j = 0; j < nbJours; j++)
                {
                    DateTime jourCourant = debutSemaine.AddDays(j);

                    // Appliquer les pertes du jour courant
                    int perteDuJour = mouvements
                        .Where(m => m.Creation.Date == jourCourant)
                        .Sum(m => m.Nombre);

                    resteActuel = Math.Max(0, resteActuel - perteDuJour);
                    joursResteSemaine.Add(resteActuel);
                }

                result.Add(joursResteSemaine);
            }
            // Console.WriteLine("\n--- FLUX JOURNALIER ---");
            // for (int s = 0; s < result.Count; s++)
            // {
            //     var jours = result[s];
            //     Console.Write($"S{s + 1}: ");

            //     int i = 0;
            //     while (i < jours.Count)
            //     {
            //         int valeur = jours[i];
            //         int debut  = i + 1; // 1-indexé

            //         // Avancer tant que la valeur est identique
            //         while (i < jours.Count && jours[i] == valeur) i++;

            //         int fin = i; // 1-indexé (i pointe déjà sur le suivant)

            //         if (debut == fin)
            //             Console.Write($"J{debut} = {valeur}");
            //         else
            //             Console.Write($"J{debut}-J{fin} = {valeur}");

            //         if (i < jours.Count) Console.Write(", ");
            //     }
            //     Console.WriteLine();
            // }
            // Console.WriteLine("--- FIN ---\n");

            return result;
        }
    }
}
