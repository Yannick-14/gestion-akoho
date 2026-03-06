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

        public async Task<Lot> createLot(Lot lot)
        {
            _dbContext.Lots.Add(lot);
            await _dbContext.SaveChangesAsync();
            return lot;
        }
    }
}
