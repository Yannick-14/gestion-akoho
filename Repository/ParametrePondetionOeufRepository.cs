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

        public async Task<ParametrePondetionOeuf> creationParametre(ParametrePondetionOeuf parametres)
        {
            _dbContext.ParametresPondetionOeuf.Add(parametres);
            await _dbContext.SaveChangesAsync();
            return parametres;
        }

        public async Task<ParametrePondetionOeuf> getParametreLotOeufPourcentage(int lotId)
        {
            return await _dbContext.ParametresPondetionOeuf
                .Where(l => l.LotOeufId == lotId)
                .FirstOrDefaultAsync();
        }
    }
}
