using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class PondetionRepository
    {
        private readonly AppDbContext _dbContext;

        public PondetionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> getNombrePendetionRace(int raceId)
        {
            return await _dbContext.Races
                .Where(r => r.Id == raceId)
                .Select(r => r.CapacitePondetion)
                .FirstOrDefaultAsync();
        }

        public async Task<int> getMaxCapacitePondetionLot(int lotId)
        {
            return await _dbContext.Lots
                .Where(l => l.Id == lotId)
                .Select(l => l.MaxCapacitePondetion)
                .FirstOrDefaultAsync();
        }
    }
}
