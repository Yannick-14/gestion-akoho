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

            return await query
                .Select(mvt => (int?)mvt.Nombre)
                .SumAsync() ?? 0;
        }

        // Recuperer juste le nombre restant dans un lot à cet instant
        public async Task<int> resteNombreRaceActuelleLot(int lotId, DateTime? dateActuelle = null)
        { 
            var lot = await _dbContext.Lots.FindAsync(lotId);
            if (lot == null) return 0;

            var dateRef = dateActuelle ?? Time.GetDateActuelle();
            int semaineActuelle = Time.getSemaineEcouler(lot.Creation, dateRef);

            if (semaineActuelle <= 1) return lot.NombreInitial;

            // Déduction retardée : On ne déduit que les morts des semaines PRÉCÉDENTES
            // Semaine 2 affiche le reste après les morts de la Semaine 1.
            int finSemainePrecedenteJours = (semaineActuelle - 1) * 7;
            DateTime dateLimite = lot.Creation.AddDays(finSemainePrecedenteJours);

            int totalMortPrecedent = await _dbContext.MouvementsLot
                .Where(mvt => mvt.LotId == lotId && mvt.Creation < dateLimite)
                .Select(mvt => (int?)mvt.Nombre)
                .SumAsync() ?? 0;

            return lot.NombreInitial - totalMortPrecedent;
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
    }
}
