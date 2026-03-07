using System;
using System.Threading.Tasks;
using AkohoAspx.Data;
using AkohoAspx.Repository;

namespace AkohoAspx.Services
{
    public class DashBoardService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly LotOeufRepository _lotOeufRepository;

        public DashBoardService()
            : this(new AppDbContext())
        {
        }

        public DashBoardService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _lotOeufRepository = new LotOeufRepository(_dbContext);
        }

        public async Task<DashboardLotItem> GetDashboardDataAsync()
        {
            var lots = await _lotRepository.GetAllAsync();
            var mouvementRepo = new MouvementLotRepository(_dbContext);
            var resteActuelLots = new System.Collections.Generic.Dictionary<int, int>();

            foreach (var lot in lots)
            {
                resteActuelLots[lot.Id] = await mouvementRepo.resteNombreRaceActuelleLot(lot.Id);
            }

            return new DashboardLotItem
            {
                LotOeufsActive = await _lotOeufRepository.GetLotOeufsActive(),
                Lots = lots,
                ResteActuelLots = resteActuelLots
            };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
