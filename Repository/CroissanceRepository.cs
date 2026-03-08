using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Models;

namespace AkohoAspx.Repository
{
    public class CroissanceRepository
    {
        private readonly AppDbContext _dbContext;

        public CroissanceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Recuperer toutes les croissance par semaine d'une race pour ses nourrtitures
        public async Task<IReadOnlyList<CroissanceAlimentRace>> getCroissanceAlimentRace(int raceId)
        {
            return await _dbContext.CroissancesAlimentRace
                .Where(c => c.RaceId == raceId)
                .ToListAsync();
        }

        // Recuperer toutes les croissance par semaine d'une race pour ses poids
        public async Task<IReadOnlyList<CroissancePoidsRace>> getCroissancePoidsRace(int raceId)
        {
            return await _dbContext.CroissancesPoidsRace
                .Where(c => c.RaceId == raceId)
                .ToListAsync();
        }

        public async Task<T> createDataEntity<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task createCroissancePoidsAndAliment(int raceId, List<CroissancePoidsRace> tabPoids, List<CroissanceAlimentRace> tabAliments)
        {
            List<CroissancePoidsRace> poidsCroissanceRace = tabPoids ?? new List<CroissancePoidsRace>();
            List<CroissanceAlimentRace> alimentCroissanceRace = tabAliments ?? new List<CroissanceAlimentRace>();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (CroissancePoidsRace race in poidsCroissanceRace)
                    {
                        var poidsRace = new CroissancePoidsRace
                        {
                            RaceId = raceId,
                            PoidsMoyen = race.PoidsMoyen,
                            ValueSemaine = race.ValueSemaine
                        };
                        await createDataEntity(poidsRace);
                    }

                    foreach (CroissanceAlimentRace aliment in alimentCroissanceRace)
                    {
                        var alimentRace = new CroissanceAlimentRace
                        {
                            RaceId = raceId,
                            PoidsMoyen = aliment.PoidsMoyen,
                            ValueSemaine = aliment.ValueSemaine
                        };
                        await createDataEntity(alimentRace);
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
