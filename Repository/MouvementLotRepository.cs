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

        public async Task<int> getTotalMortDansLot(int lotId)
        {
            return await _dbContext.MouvementsLot
                .Where(mvt => mvt.LotId == lotId)
                .Select(mvt => (int?)mvt.Nombre)
                .SumAsync() ?? 0;
        }

        public async Task<int> resteNombreRaceActuelleLot(int lotId)
        { 
            var lot = await _dbContext.Lots.FindAsync(lotId);
            if (lot == null) return 0;
            
            int totalMort = await getTotalMortDansLot(lotId);
            return lot.NombreInitial - totalMort;
        }

        public async Task<List<int>> getResteParSemaine(Lot lot)
        {
            var resteParSemaine = new List<int>();
            if (lot == null)
            {
                return resteParSemaine;
            }

            int semainesEcoulees = Time.getSemaineEcouler(lot.Creation);
            if (semainesEcoulees <= 0)
            {
                return resteParSemaine;
            }

            var mortsParSemaine = await _dbContext.MouvementsLot
                .Where(mvt => mvt.LotId == lot.Id && mvt.Creation >= lot.Creation)
                .GroupBy(mvt => DbFunctions.DiffDays(lot.Creation, mvt.Creation) / 7 + 1)
                .Select(g => new
                {
                    Semaine = g.Key,
                    Mort = g.Sum(mvt => (int?)mvt.Nombre) ?? 0
                })
                .ToListAsync();

            int resteCourant = lot.NombreInitial;

            for (int s = 1; s <= semainesEcoulees; s++)
            {
                var mortSemaine = mortsParSemaine.FirstOrDefault(v => v.Semaine == s);
                if (mortSemaine != null)
                {
                    resteCourant -= mortSemaine.Mort;
                }

                if (resteCourant < 0)
                {
                    resteCourant = 0;
                }

                resteParSemaine.Add(resteCourant);
            }

            return resteParSemaine;
        }
    }
}
