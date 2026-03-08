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
    public class LotOeufService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotOeufRepository _lotOeufRepository;
        private readonly LotRepository _lotRepository;
        private readonly RaceRepository _raceRepository;

        public LotOeufService() : this(new AppDbContext()) {}

        public LotOeufService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotOeufRepository = new LotOeufRepository(_dbContext);
            _lotRepository = new LotRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
        }

        public async Task<OperationResult> CreateLotOeuf(FormCollection requestForm)
        {
            string lotIdRaw = requestForm != null ? requestForm["lotId"] : null;
            string raceIdRaw = requestForm != null ? requestForm["raceId"] : null;
            string nombreOeufsRaw = requestForm != null ? requestForm["nombreOeufs"] : null;
    
            int.TryParse(raceIdRaw, out int raceId);
            int.TryParse(lotIdRaw, out int lotId);
            int.TryParse(nombreOeufsRaw, out int nombreOeufs);

            if (lotId <= 0 || raceId <= 0 || nombreOeufs <= 0)
                return OperationResult.Failure("Données invalides : LotId, RaceId et NombreOeufs sont obligatoires.");

            DateTime dateEclosion = Time.creationDateAvecJour(await _raceRepository.getJourEclosionRace(raceId));
            
            var lotOeuf = new LotOeuf
            {
                Creation = Time.GetDateActuelle(),
                DateEclosion = dateEclosion,
                LotParentId = lotId,
                RaceId = raceId,
                NbOeufs = nombreOeufs
            };

            try {
                await _lotOeufRepository.creationLotOeuf(lotOeuf);
                return OperationResult.Success("Lot oeuf cree avec succes.");
            } catch (Exception ex) { return OperationResult.Failure("Insertion lot échoué: " + ex.Message); }
        }

        public void Dispose() { _dbContext.Dispose(); }
    }
}
