using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;
using AkohoAspx.Services.Results;

namespace AkohoAspx.Services
{
    public class SakafoService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly RaceRepository _raceRepository;
        private readonly MouvementLotRepository _mouvementLotRepository;
        private readonly CroissanceRepository _croissanceRepository;

        public SakafoService() : this(new AppDbContext()) {}

        public SakafoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
            _mouvementLotRepository = new MouvementLotRepository(_dbContext);
            _croissanceRepository = new CroissanceRepository(_dbContext);
        }

        // compter combien se sont ecoules entre la date actuelle et la date de creation de lot
        public async Task<int> compterSemaineEcouler(int lotId)
        {
            Lot lot = await _lotRepository.getInfoIntialeLot(lotId);
            Console.WriteLine($"creation: {lot.Creation}");
            return 0;
        }

        public void Dispose() { _dbContext.Dispose(); }
    }
}
