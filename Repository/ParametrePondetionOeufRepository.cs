using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class ParametrePondetionOeufRepository
    {
        private readonly AppDbContext _dbContext;

        public ParametrePondetionOeufRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ParametrePondetionOeuf> creationLot(ParametrePondetionOeuf parametres)
        {
            _dbContext.ParametresPondesionOeuf.Add(parametres);
            await _dbContext.SaveChangesAsync();
            return parametres;
        }

        public async Task<ParametrePondetionOeuf> getInfoIntialeLot(int lotId)
        {
            return await _dbContext.ParametresPondesionOeuf
                .Where(l => l.LotOeufId == lotId)
                .FirstOrDefaultAsync();
        }
    }
}
