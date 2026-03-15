using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;
using AkohoAspx.Utils;
using AkohoAspx.Services.Results;
using AkohoAspx.Services.Pondetion;

namespace AkohoAspx.Services
{
    public class LotOeufService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotOeufRepository _lotOeufRepository;
        private readonly LotRepository _lotRepository;
        private readonly RaceRepository _raceRepository;
        private readonly PondetionService pondetionService;

        public LotOeufService() : this(new AppDbContext()) {}

        public LotOeufService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotOeufRepository = new LotOeufRepository(_dbContext);
            _lotRepository = new LotRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
            pondetionService = new PondetionService(); 
        }

        public async Task<OperationResult> CreateLotOeuf(FormCollection requestForm)
        {
            string lotIdRaw = requestForm != null ? requestForm["lotId"] : null;
            string raceIdRaw = requestForm != null ? requestForm["raceId"] : null;
            string nombreOeufsRaw = requestForm != null ? requestForm["nombreOeufs"] : null;
    
            int.TryParse(raceIdRaw, out int raceId);
            int.TryParse(lotIdRaw, out int lotId);
            int.TryParse(nombreOeufsRaw, out int nombreOeufs);

            DateTime dateActuelle = Time.GetDateActuelle();

            Console.WriteLine($"[LotOeufService] Tentative création : LotId={lotId}, RaceId={raceId}, Nb={nombreOeufs}");

            if (lotId <= 0 || raceId <= 0 || nombreOeufs <= 0)
            {
                Console.WriteLine("[LotOeufService] Erreur : Données d'entrée invalides (<= 0)");
                return OperationResult.Failure("Données invalides : LotId, RaceId et NombreOeufs sont obligatoires.");
            }

            bool check = await pondetionService.checkPossibilitePondetion(lotId, nombreOeufs, dateActuelle);
            if (!check)
            {
                return OperationResult.Failure("Capacite maximum de pondu d'oeuf atteint dans ce lot");
            }

            DateTime dateEclosion = Time.creationDateAvecJour(await _raceRepository.getJourEclosionRace(raceId));
            
            var lotOeuf = new LotOeuf
            {
                Creation = dateActuelle,
                DateEclosion = dateEclosion,
                LotParentId = lotId,
                RaceId = raceId,
                NbOeufs = nombreOeufs
            };

            Console.WriteLine($"[LotOeufService] Objet prêt : Creation={lotOeuf.Creation}, Eclosion={lotOeuf.DateEclosion}");

            try {
                await _lotOeufRepository.creationLotOeuf(lotOeuf);
                Console.WriteLine("[LotOeufService] Insertion réussie !");
                return OperationResult.Success("Lot oeuf créé avec succès.");
            } catch (Exception ex) { 
                Console.WriteLine($"[LotOeufService] EXCEPTION CRITIQUE : {ex.Message}\n{ex.StackTrace}");
                return OperationResult.Failure("Insertion lot échoué: " + ex.Message); 
            }
        }

        public void Dispose() { _dbContext.Dispose(); }
    }
}
