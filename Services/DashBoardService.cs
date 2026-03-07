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
            return new DashboardLotItem
            {
                LotOeufsActive = await _lotOeufRepository.GetLotOeufsActive(),
                Lots = await _lotRepository.GetAllAsync()
            };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
