using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class PrixVenteAtodyRepository
    {
        private readonly AppDbContext _dbContext;

        public PrixVenteAtodyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PrixVenteAtody> GetPrixVenteAtody(int raceId)
        {
            return await _dbContext.PrixVentesAtody
                .Where(p => p.RaceId == raceId)
                .OrderByDescending(p => p.Creation)
                .FirstOrDefaultAsync();
        }
    }
}
