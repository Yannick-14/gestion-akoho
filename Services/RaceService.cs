using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;
using AkohoAspx.Services.Results;

namespace AkohoAspx.Services
{
    public class RaceService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly RaceRepository _raceRepository;
        private readonly CroissanceRepository _croissanceRepository;
        private readonly PrixVenteRacePoidsRepository _prixVenteRacePoidsRepository;

        public RaceService()
            : this(new AppDbContext())
        {
        }

        public RaceService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _raceRepository = new RaceRepository(_dbContext);
            _croissanceRepository = new CroissanceRepository(_dbContext);
            _prixVenteRacePoidsRepository = new PrixVenteRacePoidsRepository(_dbContext);
        }

        public async Task<RaceIndexData> GetIndexDataAsync(object currentRaceSessionValue)
        {
            IReadOnlyList<Race> races = await _raceRepository.GetAllAsync();
            return new RaceIndexData
            {
                Races = races,
                CurrentRaceId = ResolveRaceId(currentRaceSessionValue)
            };
        }

        public async Task<OperationResult<int>> CreateRaceAsync(string nom, int jourFoyAtody)
        {
            if (string.IsNullOrWhiteSpace(nom) || jourFoyAtody <= 0)
            {
                return OperationResult<int>.Failure("Le nom et le jour de foy atody sont obligatoires.");
            }

            var race = new Race
            {
                Nom = nom.Trim(),
                JourFoyAtody = jourFoyAtody
            };

            int createdRaceId = await _raceRepository.CreateAsync(race);
            return OperationResult<int>.Success(createdRaceId, "Race creee. RaceId actif: " + createdRaceId);
        }

        public async Task<OperationResult> CreateCroissanceRaceAsync(
            int raceId,
            object currentRaceSessionValue,
            IList<CroissancePoidsRace> leftItems,
            IList<CroissanceAlimentRace> rightItems)
        {
            int resolvedRaceId = ResolveRequestedRaceId(raceId, currentRaceSessionValue);
            if (resolvedRaceId <= 0)
            {
                return OperationResult.Failure("Aucune race active dans la session.");
            }

            List<CroissancePoidsRace> left = leftItems != null
                ? new List<CroissancePoidsRace>(leftItems)
                : new List<CroissancePoidsRace>();
            List<CroissanceAlimentRace> right = rightItems != null
                ? new List<CroissanceAlimentRace>(rightItems)
                : new List<CroissanceAlimentRace>();

            await _croissanceRepository.createCroissancePoidsAndAliment(resolvedRaceId, left, right);
            return OperationResult.Success((left.Count + right.Count) + " ligne(s) inseree(s) pour la race " + resolvedRaceId + ".");
        }

        public async Task<OperationResult> AddPrixUnitaireAsync(string raceIdRaw, string prixRaw, object currentRaceSessionValue)
        {
            int resolvedRaceId = ResolveRequestedRaceId(ParseInt(raceIdRaw), currentRaceSessionValue);
            if (resolvedRaceId <= 0)
            {
                return OperationResult.Failure("Aucune race active dans la session.");
            }

            decimal prix = ParseDecimal(prixRaw);
            await _prixVenteRacePoidsRepository.Creation(new PrixVenteRaceParPoids
            {
                RaceId = resolvedRaceId,
                Prix = prix
            });

            return OperationResult.Success("Prix enregistre pour la race " + resolvedRaceId + ": " + prix);
        }

        public OperationResult BuildResetCurrentRaceResult()
        {
            return OperationResult.Success("Race active retiree de la session.");
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private static int ResolveRequestedRaceId(int raceId, object currentRaceSessionValue)
        {
            return raceId > 0 ? raceId : ResolveRaceId(currentRaceSessionValue);
        }

        private static int ResolveRaceId(object currentRaceSessionValue)
        {
            if (currentRaceSessionValue == null)
            {
                return 0;
            }

            int raceId;
            return int.TryParse(currentRaceSessionValue.ToString(), out raceId) ? raceId : 0;
        }

        private static int ParseInt(string rawValue)
        {
            int value;
            return int.TryParse(rawValue, out value) ? value : 0;
        }

        private static decimal ParseDecimal(string rawValue)
        {
            decimal value;
            string safeValue = rawValue ?? "0";

            bool parsed = decimal.TryParse(safeValue, NumberStyles.Number, CultureInfo.CurrentCulture, out value)
                          || decimal.TryParse(safeValue, NumberStyles.Number, CultureInfo.InvariantCulture, out value);

            return parsed ? value : 0m;
        }
    }
}
