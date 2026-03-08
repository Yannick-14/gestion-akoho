using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class PrixVenteRacePoidsRepository
    {
        private readonly AppDbContext _dbContext;

        public PrixVenteRacePoidsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<PrixVenteRace>> GetAllAsync()
        {
            return await _dbContext.PrixVentesRace.ToListAsync();
        }

        public async Task<IReadOnlyList<PrixVenteRace>> getPrixVenteRace(int raceId)
        {
            return await _dbContext.PrixVentesRace
                .Where(p => p.RaceId == raceId)
                .OrderByDescending(p => p.Creation)
                .ToListAsync();
        }

        public async Task<PrixVenteRace> Creation(PrixVenteRace prixVenteRaceParPoids)
        {
            _dbContext.PrixVentesRace.Add(prixVenteRaceParPoids);
            await _dbContext.SaveChangesAsync();
            return prixVenteRaceParPoids;
        }

        public async Task<bool> ExistsAsync(int raceId)
        {
            return await _dbContext.Races.AnyAsync(r => r.Id == raceId);
        }
    }
}
