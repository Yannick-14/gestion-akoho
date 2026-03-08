using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class PrixVenteRaceRepository
    {
        private readonly AppDbContext _dbContext;

        public PrixVenteRaceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PrixVenteRace> GetLatestPrixVenteByRaceId(int raceId, DateTime dateActuelle)
        {
            return await _dbContext.PrixVentesRace
                .Where(p => p.RaceId == raceId && p.Creation <= dateActuelle)
                .OrderByDescending(p => p.Creation)
                .FirstOrDefaultAsync();
        }
    }
}
