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
    public class MouvementLotService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly RaceRepository _raceRepository;
        private readonly MouvementLotRepository _mouvementLotRepository;

        public MouvementLotService() : this(new AppDbContext()) {}
        public int getPoidsDefault() { return 150; }

        public MouvementLotService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
            _mouvementLotRepository = new MouvementLotRepository(_dbContext);
        }

        public async Task<OperationResult> creationMouvement(FormCollection requestForm)
        {
            string lotIdRaw = requestForm != null ? requestForm["lotId"] : null;
            string nombreInsererRaw = requestForm != null ? requestForm["NombreSortie"] : null;

            int.TryParse(lotIdRaw, out int lotId);
            int.TryParse(nombreInsererRaw, out int nombreInserer);

            var mouvement = new MouvementLot {
                Creation = Time.GetDateActuelle(),
                LotId = lotId,
                Nombre = nombreInserer
            };
            try {
                await _mouvementLotRepository.creationMouvement(mouvement);
                return OperationResult.Success("Success de transaction.");
            } catch (Exception ex) { return OperationResult.Failure("Erreur de transaction: " + ex.Message); }
        }

        /*Restreint le jour où il y a une perte et l'exclure de la semaine
        *Recuperer les jours de cette où il n'y a pas de perte
        *Recuperer la totalite
        */

        public void Dispose() { _dbContext.Dispose(); }
    }
}
