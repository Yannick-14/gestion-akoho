using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class LotOeufRepository
    {
        private readonly AppDbContext _dbContext;

        public LotOeufRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LotOeuf> creationLotOeuf(LotOeuf lot)
        {
            _dbContext.LotsOeufs.Add(lot);
            await _dbContext.SaveChangesAsync();
            return lot;
        }

        public async Task<IReadOnlyList<LotOeuf>> GetAllAsync()
        {
            return await _dbContext.LotsOeufs
                .Include(l => l.Race)
                .OrderByDescending(l => l.Creation)
                .ToListAsync();
        }
        public async Task<IReadOnlyList<LotOeuf>> GetLotOeufsActive()
        {
            return await _dbContext.LotsOeufs
                .Include(l => l.Race)
                .Where(l => l.validation == false)
                .OrderByDescending(l => l.Creation)
                .ToListAsync();
        }

        public async Task<LotOeuf> getInfoIntialeLotOeufs(int lotId)
        {
            return await _dbContext.LotsOeufs
                .Where(l => l.Id == lotId)
                .FirstOrDefaultAsync();
        }
    }
}
