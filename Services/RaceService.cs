using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
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

        public RaceService() : this(new AppDbContext()) {}

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

        public async Task<OperationResult<int>> CreateRaceAsync(FormCollection requestForm)
        {
            string nom = (requestForm != null ? requestForm["nom"] : null) ?? string.Empty;
            int jourFoyAtody = ParseInt(requestForm != null ? requestForm["jourFoyAtody"] : null);

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

        public async Task<OperationResult> CreateCroissanceRaceAsync(FormCollection requestForm, object currentRaceSessionValue)
        {
            int raceId = ParseInt(requestForm != null ? requestForm["raceId"] : null);

            List<CroissancePoidsRace> left = ParseCroissancePoidsItems(requestForm);
            List<CroissanceAlimentRace> right = ParseCroissanceAlimentItems(requestForm);

            await _croissanceRepository.createCroissancePoidsAndAliment(raceId, left, right);
            return OperationResult.Success((left.Count + right.Count) + " ligne(s) inseree(s) pour la race " + resolvedRaceId + ".");
        }

        public async Task<OperationResult> AddPrixUnitaireAsync(FormCollection requestForm, object currentRaceSessionValue)
        {
            string raceIdRaw = requestForm != null ? requestForm["raceId"] : null;
            string prixRaw = requestForm != null ? requestForm["prix"] : null;
            int resolvedRaceId = ResolveRequestedRaceId(ParseInt(raceIdRaw), currentRaceSessionValue);
            if (resolvedRaceId <= 0) return OperationResult.Failure("Aucune race active dans la session.");

            decimal prix = ParseDecimal(prixRaw);
            await _prixVenteRacePoidsRepository.Creation(new PrixVenteRaceParPoids
            {
                RaceId = resolvedRaceId,
                Prix = prix
            });

            return OperationResult.Success("Prix enregistre pour la race " + resolvedRaceId + ": " + prix);
        }

        public OperationResult BuildResetCurrentRaceResult() { return OperationResult.Success("Race active retiree de la session."); }

        public void Dispose()
        {
            _dbContext.Dispose();
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

        private static List<CroissancePoidsRace> ParseCroissancePoidsItems(FormCollection requestForm)
        {
            var items = new List<CroissancePoidsRace>();
            foreach (int index in ExtractIndexes(requestForm, "leftItems["))
            {
                string semaine = requestForm["leftItems[" + index + "].Semaine"];
                int poids = ParseInt(requestForm["leftItems[" + index + "].Poids"]);

                items.Add(new CroissancePoidsRace
                {
                    Semaine = (semaine ?? string.Empty).Trim(),
                    Poids = poids
                });
            }

            return items;
        }

        private static List<CroissanceAlimentRace> ParseCroissanceAlimentItems(FormCollection requestForm)
        {
            var items = new List<CroissanceAlimentRace>();
            foreach (int index in ExtractIndexes(requestForm, "rightItems["))
            {
                string semaine = requestForm["rightItems[" + index + "].Semaine"];
                int aliment = ParseInt(requestForm["rightItems[" + index + "].Aliment"]);

                items.Add(new CroissanceAlimentRace
                {
                    Semaine = (semaine ?? string.Empty).Trim(),
                    Aliment = aliment
                });
            }

            return items;
        }

        private static SortedSet<int> ExtractIndexes(FormCollection requestForm, string collectionPrefix)
        {
            var indexes = new SortedSet<int>();
            if (requestForm == null || requestForm.AllKeys == null)
            {
                return indexes;
            }

            foreach (string key in requestForm.AllKeys)
            {
                if (string.IsNullOrWhiteSpace(key) || !key.StartsWith(collectionPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                int indexStart = collectionPrefix.Length;
                int indexEnd = key.IndexOf(']', indexStart);
                if (indexEnd <= indexStart)
                {
                    continue;
                }

                string indexRaw = key.Substring(indexStart, indexEnd - indexStart);
                int index;
                if (int.TryParse(indexRaw, out index))
                {
                    indexes.Add(index);
                }
            }

            return indexes;
        }
    }
}
