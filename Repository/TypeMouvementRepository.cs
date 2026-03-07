using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class TypeMouvementRepository
    {
        private readonly AppDbContext _dbContext;

        public TypeMouvementRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<TypeMouvement>> getTypeMouvement(string type)
        {
            return await _dbContext.TypesMouvement
                .Where(tm => tm.Nom == type)
                .ToListAsync();
        }
    }
}
