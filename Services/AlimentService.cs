using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;
using AkohoAspx.Utils;
using AkohoAspx.Services.Results;

namespace AkohoAspx.Services
{
    public class AlimentService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly LotOeufRepository _lotOeufRepository;
        private readonly RaceRepository _raceRepository;
        private readonly MouvementLotRepository _mouvementLotRepository;

        public AlimentService() : this(new AppDbContext()) {}

        public AlimentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _lotOeufRepository = new LotOeufRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
            _mouvementLotRepository = new MouvementLotRepository(_dbContext);
        }

        // Calculer combien d'aliment peut on servir dans x jour ex: J1-J3, x = 3, poidsHebdo = 50g, => res = poidsHebdo / x
        public decimal getPoidsAlimentXjours(decimal poidsHebdo, int nbJourDef)
        {
            return (decimal) (poidsHebdo / 7M) * nbJourDef;
        }

        public void Dispose() { _dbContext.Dispose(); }
    }
}
