using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class LotRepository
    {
        private readonly AppDbContext _dbContext;

        public LotRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Lot> creationLot(Lot lot)
        {
            _dbContext.Lots.Add(lot);
            await _dbContext.SaveChangesAsync();
            return lot;
        }

        public async Task<IReadOnlyList<Lot>> GetAllAsync()
        {
            return await _dbContext.Lots
                .Include(l => l.Race)
                .OrderByDescending(l => l.Creation)
                .ToListAsync();
        }

        public async Task<Lot> getInfoIntialeLot(int lotId)
        {
            return await _dbContext.Lots
                .Where(l => l.Id == lotId)
                .FirstOrDefaultAsync();
        }
    }
}
