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
        private readonly LotOeufRepository _lotOeufRepository;
        private readonly RaceRepository _raceRepository;
        private readonly MouvementLotRepository _mouvementLotRepository;

        public LotService() : this(new AppDbContext()) {}
        public int getPoidsDefault() { return 150; }

        public LotService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _lotOeufRepository = new LotOeufRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
            _mouvementLotRepository = new MouvementLotRepository(_dbContext);
        }

        public async Task<LotIndexData> GetIndexDataAsync()
        {
            return new LotIndexData
            {
                Lots = await _lotRepository.GetAllAsync(),
                Races = await _raceRepository.GetAllAsync()
            };
        }

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

            if (string.IsNullOrWhiteSpace(nomLot) || raceId <= 0 || nombreInitial <= 0 || poidsAchat <= 0) return OperationResult.Failure("Donnees invalides. Verifiez les champs du formulaire.");

            if (!await _raceRepository.ExistsAsync(raceId)) return OperationResult.Failure("Race introuvable.");

            var lot = new Lot
            {
                NomLot = nomLot,
                RaceId = raceId,
                NombreInitial = nombreInitial,
                PoidsInitiale = poidsAchat,
                PrixAchat = totalInvesti,
                Creation = DateTime.Now
            };

            try
            {
                await _lotRepository.creationLot(lot);
                return OperationResult.Success("Lot cree avec succes.");
            } catch (Exception ex) { return OperationResult.Failure("Insertion lot echouee: " + ex.Message); }
        }

        // creer un lot provennant d'extraction d'atody
        public async Task<OperationResult> CreateNewLotFromLotOeuf(FormCollection requestForm)
        {
            string lotOeufIdRaw = requestForm != null ? requestForm["lotOeufId"] : null;
            string pourcentageRaw = requestForm != null ? requestForm["pourcentage"] : null;
            string nomLotRaw = requestForm != null ? requestForm["nomLot"] : null;

            string newNomLot = (nomLotRaw ?? string.Empty).Trim();

            int.TryParse(lotOeufIdRaw, out int lotOeufId);
            decimal.TryParse(pourcentageRaw, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal pourcentage);

            if (lotOeufId <= 0 || pourcentage <= 0 || string.IsNullOrWhiteSpace(newNomLot)) return OperationResult.Failure("Données invalides : LotOeufId, Pourcentage et NomLot sont obligatoires.");

            LotOeuf lotOeuf = await _lotOeufRepository.getInfoIntialeLotOeufs(lotOeufId);
            if (lotOeuf == null) return OperationResult.Failure("LotOeuf introuvable.");

            int oeufsEclos = resultTotalEclos(lotOeuf.NbOeufs, pourcentage);
            Race detailRace = await _raceRepository.getInfoRace(lotOeuf.RaceId);
            var newLot = new Lot
            {
                Creation = DateTime.Now,
                NomLot = newNomLot,
                RaceId = lotOeuf.RaceId,
                NombreInitial = oeufsEclos,
                PoidsInitiale = detailRace.PoidsDefaut,
                PrixAchat = 0,
                LotOeufId = lotOeuf.Id
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _lotRepository.creationLot(newLot);
                    await _lotOeufRepository.updateValidationEtPourcentage(lotOeuf.Id, true, pourcentage);
                    transaction.Commit();
                    return OperationResult.Success("Lot créé avec succès à partir du lot d'Oeufs.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return OperationResult.Failure("Insertion lot échouée (rollback effectué) : " + ex.Message);
                }
            }
        }

        public int resultTotalEclos(int nombreOeufs, decimal pourcentage)
        {
            // Nombre entier d'oeufs éclos = floor(nbOeufs * pourcentage / 100)
            return (int)Math.Floor(nombreOeufs * pourcentage / 100m);
        }

        public void Dispose() { _dbContext.Dispose(); }
    }
}
