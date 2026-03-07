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
    public class LotService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly RaceRepository _raceRepository;
        private readonly MouvementLotRepository _mouvementLotRepository;
        private readonly TypeMouvementRepository _typeMouvementRepository;

        public LotService()
            : this(new AppDbContext())
        {
        }

        public LotService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
            _mouvementLotRepository = new MouvementLotRepository(_dbContext);
            _typeMouvementRepository = new TypeMouvementRepository(_dbContext);
        }

        public async Task<LotIndexData> GetIndexDataAsync()
        {
            return new LotIndexData
            {
                Lots = await _lotRepository.GetAllAsync(),
                Races = await _raceRepository.GetAllAsync()
            };
        }

        public int getPoidsDefault() { return 150; }

        public async Task<OperationResult> CreateLotAsync(FormCollection requestForm)
        {
            string nomLotRaw = requestForm != null ? requestForm["nomLot"] : null;
            string raceIdRaw = requestForm != null ? requestForm["raceId"] : null;
            string nombreInitialRaw = requestForm != null ? requestForm["nombreInitial"] : null;
            string poidsAchatRaw = requestForm != null ? requestForm["poidsAchat"] : null;
            string totalInvestiRaw = requestForm != null ? requestForm["totalInvesti"] : null;

            string nomLot = (nomLotRaw ?? string.Empty).Trim();

            int.TryParse(raceIdRaw, out int raceId);
            int.TryParse(nombreInitialRaw, out int nombreInitial);
            int.TryParse(poidsAchatRaw, out int poidsAchat);

            string totalRaw = totalInvestiRaw ?? "0";
            decimal totalInvesti;
            bool totalOk = decimal.TryParse(totalRaw, NumberStyles.Number, CultureInfo.CurrentCulture, out totalInvesti) || decimal.TryParse(totalRaw, NumberStyles.Number, CultureInfo.InvariantCulture, out totalInvesti);

            if (string.IsNullOrWhiteSpace(nomLot) || raceId <= 0 || nombreInitial <= 0 || poidsAchat <= 0)
            {
                return OperationResult.Failure("Donnees invalides. Verifiez les champs du formulaire.");
            }

            if (!await _raceRepository.ExistsAsync(raceId))
            {
                return OperationResult.Failure("Race introuvable.");
            }

            var lot = new Lot
            {
                NomLot = nomLot,
                RaceId = raceId,
                NombreInitial = nombreInitial,
                PoidsAchat = poidsAchat,
                TotalInvesti = totalInvesti,
                Creation = DateTime.Now,
                Statu = 0
            };

            try
            {
                Lot resultLot = await _lotRepository.creationLot(lot);
                var mouvement = new MouvementLot
                {
                    LotId = resultLot.Id,
                    Quantite = nombreInitial,
                    Creation = DateTime.Now,
                    TypeId = await _typeMouvementRepository.getIdMouvementEntree()
                };
                await _mouvementLotRepository.creationMouvement(mouvement);
                return OperationResult.Success("Lot cree avec succes.");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Insertion lot echouee: " + ex.Message);
            }
        }

        // creer un lot provennant d'extraction d'atody
        public async Task<OperationResult> CreateLotAtody(FormCollection requestForm)
        {
            string nomLotRaw = requestForm != null ? requestForm["nomLot"] : null;
            string raceIdRaw = requestForm != null ? requestForm["raceId"] : null;
            string lotIdRaw = requestForm != null ? requestForm["lotId"] : null;
            string quantiteAtodyRaw = requestForm != null ? requestForm["quantiteAtody"] : null;

            string nomLot = (nomLotRaw ?? string.Empty).Trim();

            int.TryParse(raceIdRaw, out int raceId);
            int.TryParse(lotIdRaw, out int lotId);
            int.TryParse(quantiteAtodyRaw, out int quantiteAtody);

            if (string.IsNullOrWhiteSpace(nomLot) || raceId <= 0 || quantiteAtody <= 0)
            {
                return OperationResult.Failure("Donnees invalides. Verifiez les champs du formulaire.");
            }

            if (!await _raceRepository.ExistsAsync(raceId))
            {
                return OperationResult.Failure("Race introuvable.");
            }
            DateTime dateEclosion = Time.creationDateAvecJour(await _raceRepository.getJourEclosionRace(raceId));
            var newLot = new Lot
            {
                NomLot = nomLot,
                RaceId = raceId,
                NombreInitial = quantiteAtody,
                PoidsAchat = getPoidsDefault(),
                TotalInvesti = 0,
                Creation = DateTime.Now,
                DateAfoyAkoho = dateEclosion,
                LotParent = lotId,
                Statu = 1
            };
            try
            {
                Lot resultLot = await _lotRepository.creationLot(newLot);
                var mouvement = new MouvementLot
                {
                    LotId = resultLot.Id,
                    Quantite = quantiteAtody,
                    Creation = DateTime.Now,
                    TypeId = await _typeMouvementRepository.getIdMouvementEntree()
                };
                await _mouvementLotRepository.creationMouvement(mouvement);
                return OperationResult.Success("Nouvel lot cree avec succes.");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Insertion lot echouee: " + ex.Message);
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
