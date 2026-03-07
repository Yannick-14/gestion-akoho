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
    }
}
