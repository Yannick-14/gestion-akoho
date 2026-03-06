using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<IReadOnlyList<PrixVenteRaceParPoids>> GetAllAsync()
        {
            return await _dbContext.PrixVentesRaceParPoids.ToListAsync();
        }

        public async Task<PrixVenteRaceParPoids> Creation(PrixVenteRaceParPoids prixVenteRaceParPoids)
        {
            _dbContext.PrixVentesRaceParPoids.Add(prixVenteRaceParPoids);
            await _dbContext.SaveChangesAsync();
            return prixVenteRaceParPoids;
        }

        public async Task<bool> ExistsAsync(int raceId)
        {
            return await _dbContext.Races.AnyAsync(r => r.Id == raceId);
        }
    }
}
