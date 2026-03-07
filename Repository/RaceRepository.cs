using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class RaceRepository
    {
        private readonly AppDbContext _dbContext;

        public RaceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Race>> GetAllAsync()
        {
            return await _dbContext.Races.ToListAsync();
        }

        public async Task<int> CreateAsync(Race race)
        {
            _dbContext.Races.Add(race);
            await _dbContext.SaveChangesAsync();
            return race.Id;
        }

        public async Task<bool> ExistsAsync(int raceId)
        {
            return await _dbContext.Races.AnyAsync(r => r.Id == raceId);
        }

        public async Task<int> getJourEclosionRace(int raceId)
        {
            return await _dbContext.Races
                .Where(r => r.Id == raceId)
                .Select(r => r.DureEclosionOeuf)
                .FirstOrDefaultAsync();
        }
    }
}
