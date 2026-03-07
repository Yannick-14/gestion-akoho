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
    public class LotService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly RaceRepository _raceRepository;

        public LotService()
            : this(new AppDbContext())
        {
        }

        public LotService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
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

            int raceId;
            int nombreInitial;
            int poidsAchat;
            decimal totalInvesti;

            bool raceOk = int.TryParse(raceIdRaw, out raceId);
            bool nombreOk = int.TryParse(nombreInitialRaw, out nombreInitial);
            bool poidsOk = int.TryParse(poidsAchatRaw, out poidsAchat);

            string totalRaw = totalInvestiRaw ?? "0";
            bool totalOk = decimal.TryParse(totalRaw, NumberStyles.Number, CultureInfo.CurrentCulture, out totalInvesti) || decimal.TryParse(totalRaw, NumberStyles.Number, CultureInfo.InvariantCulture, out totalInvesti);

            if (string.IsNullOrWhiteSpace(nomLot) || !raceOk || raceId <= 0 || !nombreOk || !poidsOk || !totalOk)
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
                await _lotRepository.creationLot(lot);
                return OperationResult.Success("Lot cree avec succes.");
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
