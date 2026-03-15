using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;
using AkohoAspx.Utils;
using AkohoAspx.Services.Results;

namespace AkohoAspx.Services.Pondetion
{
    public class PondetionService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly PondetionRepository _pondetionRepository;
        private readonly LotOeufRepository _lotOeufRepository;

        public PondetionService() : this(new AppDbContext()) {}

        public PondetionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _pondetionRepository = new PondetionRepository(_dbContext);
            _lotOeufRepository = new LotOeufRepository(_dbContext);
        }

        public async Task<bool> checkPossibilitePondetion(int lotId, int oeufsExtrait, DateTime dateActuelle)
        {
            int totalitePonduActualiser = await _lotOeufRepository.totaliteOeufsLotPondu(lotId, dateActuelle);
            int maxPondetionLot = await _pondetionRepository.getMaxCapacitePondetionLot(lotId);
            
            return (maxPondetionLot - totalitePonduActualiser) >= oeufsExtrait;
        }

        public void Dispose() { _dbContext.Dispose(); }
        // traitement de demande d'extraction d'oeufs
    }
}
