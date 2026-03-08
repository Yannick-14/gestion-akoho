using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class PrixNourritureRaceRepository
    {
        private readonly AppDbContext _dbContext;

        public PrixNourritureRaceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<decimal> getPrixNourritureRaceId(raceId)
        {
            return await _dbContext.PrixNourrituresRace
                .Where(p => p.RaceId == raceId)
                .OrderByDescending(p => p.Creation)
                .Select(p => p.Prix)
                .FirstOrDefaultAsync();
        }
    }
}
