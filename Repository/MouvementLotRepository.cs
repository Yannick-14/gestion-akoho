using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class MouvementLotRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly TypeMouvementRepository _typeMouvementRepository;

        public MouvementLotRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _typeMouvementRepository = new TypeMouvementRepository(dbContext);
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

        public async Task<int> getResteTotalLot(int lotId)
        {
            int idEntree = await _typeMouvementRepository.getIdMouvementEntree();
            int idSortie = await _typeMouvementRepository.getIdMouvementSortie();

            return await _dbContext.MouvementsLot
                .Where(mvt => mvt.LotId == lotId)
                .Select(mvt => (int?)(mvt.TypeId == idEntree ? mvt.Quantite : (mvt.TypeId == idSortie ? -mvt.Quantite : 0)))
                .SumAsync() ?? 0;
        }

        public async Task<int> getResteMortTotalLot(int lotId)
        {
            int idSortie = await _typeMouvementRepository.getIdMouvementSortie();

            return await _dbContext.MouvementsLot
                .Where(mvt => mvt.LotId == lotId)
                .Select(mvt => (int?)(mvt.TypeId == idSortie ? mvt.Quantite : 0))
                .SumAsync() ?? 0;
        }

        public async Task<List<int>> getResteParSemaine(int lotId, System.DateTime lotCreationDate, int semainesEcoulees)
        {
            int idEntree = await _typeMouvementRepository.getIdMouvementEntree();
            int idSortie = await _typeMouvementRepository.getIdMouvementSortie();

            // Grouping by week using database functions for efficient SQL aggregation
            var variations = await _dbContext.MouvementsLot
                .Where(mvt => mvt.LotId == lotId && mvt.Creation >= lotCreationDate)
                .GroupBy(mvt => System.Data.Entity.DbFunctions.DiffDays(lotCreationDate, mvt.Creation) / 7 + 1)
                .Select(g => new
                {
                    Semaine = g.Key,
                    Variation = g.Sum(mvt => mvt.TypeId == idEntree ? mvt.Quantite : (mvt.TypeId == idSortie ? -mvt.Quantite : 0))
                })
                .ToListAsync();

            var resteParSemaine = new List<int>();
            int resteCourant = 0;

            for (int s = 1; s <= semainesEcoulees; s++)
            {
                var variationSemaine = variations.FirstOrDefault(v => v.Semaine == s);
                if (variationSemaine != null)
                {
                    resteCourant += variationSemaine.Variation;
                }
                resteParSemaine.Add(resteCourant);
            }

            return resteParSemaine;
        }
    }
}
