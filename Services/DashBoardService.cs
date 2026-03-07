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
    public class DashBoardService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly RaceRepository _raceRepository;
        private readonly MouvementLotRepository _mouvementLotRepository;
        private readonly PrixVenteRacePoidsRepository _prixVenteRacePoidsRepository;
        private readonly SakafoService _sakafoService;

        public DashBoardService() : this(new AppDbContext()) {}

        public DashBoardService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
            _mouvementLotRepository = new MouvementLotRepository(_dbContext);
            _prixVenteRacePoidsRepository = new PrixVenteRacePoidsRepository(_dbContext);
            _sakafoService = new SakafoService(_dbContext);
        }

        public async Task<int> getMortTotalDansLot(int lotId) { return await _mouvementLotRepository.getResteMortTotalLot(lotId); }

        public async Task<System.Collections.Generic.IEnumerable<DashboardLotItem>> GetDashboardDataAsync()
        {
            var lots = await _lotRepository.GetAllAsync();
            var items = new System.Collections.Generic.List<DashboardLotItem>();

            foreach (var lot in lots)
            {
                var initialInfo = await _lotRepository.getInfoIntialeLot(lot.Id);

                if (initialInfo == null) continue;

                int reste = await _mouvementLotRepository.getResteTotalLot(lot.Id);

                var prixVentes = await _prixVenteRacePoidsRepository.getPrixVenteRace(lot.RaceId);
                decimal prixMoyen = System.Linq.Enumerable.Any(prixVentes) ? System.Linq.Enumerable.Average(prixVentes, p => p.Prix) : 0;

                int mort = await getMortTotalDansLot(lot.Id);
                decimal benefice = (reste * prixMoyen) - initialInfo.TotalInvesti;
                decimal totaliteGramme = await _sakafoService.getDepenseAlimentActuelleLotEnGramme(lot.Id);
                decimal prixMoyenSakafo = 1000; // 1000Ar 20g
                decimal depenseSakafo = (totaliteGramme * prixMoyenSakafo) / 20;

                items.Add(new DashboardLotItem
                {
                    Lot = initialInfo,
                    ResteNombreActuel = reste,
                    Mort = mort,
                    Benefice = benefice,
                    DepenseSakafo = depenseSakafo
                });
            }

            return items;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
