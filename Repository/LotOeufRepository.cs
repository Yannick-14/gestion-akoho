using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class LotOeufRepository
    {
        private readonly AppDbContext _dbContext;

        public LotOeufRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LotOeuf> creationLotOeuf(LotOeuf lot)
        {
            _dbContext.LotsOeuf.Add(lot);
            await _dbContext.SaveChangesAsync();
            return lot;
        }

        public async Task<IReadOnlyList<LotOeuf>> GetAllAsync()
        {
            return await _dbContext.LotsOeuf
                .Include(l => l.Race)
                .OrderByDescending(l => l.Creation)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<LotOeuf>> GetLotOeufsActive()
        {
            return await _dbContext.LotsOeuf
                .Include(l => l.Race)
                .Where(l => l.Validation == false)
                .OrderByDescending(l => l.Creation)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<LotOeuf>> GetLotOeufsActive(DateTime dateActuelle)
        {
            return await _dbContext.LotsOeuf
                .Include(l => l.Race)
                .Where(l => l.Validation == false && l.Creation <= dateActuelle)
                .OrderByDescending(l => l.Creation)
                .ToListAsync();
        }

        public async Task<LotOeuf> getInfoIntialeLotOeufs(int lotId)
        {
            return await _dbContext.LotsOeuf
                .Where(l => l.Id == lotId)
                .FirstOrDefaultAsync();
        }

        public async Task<LotOeuf> updateValidationEtPourcentage(int lotId, bool validation, decimal pourcentage)
        {
            LotOeuf lotOeuf = await _dbContext.LotsOeuf
                .Where(l => l.Id == lotId)
                .FirstOrDefaultAsync();

            if (lotOeuf == null) return null;

            lotOeuf.Validation = validation;
            lotOeuf.Pourcentage = pourcentage;

            await _dbContext.SaveChangesAsync();
            return lotOeuf;
        }
    }
}
