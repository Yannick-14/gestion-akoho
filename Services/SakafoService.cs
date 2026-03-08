using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Data;
using AkohoAspx.Models;
using AkohoAspx.Repository;
using AkohoAspx.Services.Results;
using AkohoAspx.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AkohoAspx.Services
{
    public class SakafoService : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly LotRepository _lotRepository;
        private readonly RaceRepository _raceRepository;

        public SakafoService() : this(new AppDbContext()) {}

        public SakafoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _lotRepository = new LotRepository(_dbContext);
            _raceRepository = new RaceRepository(_dbContext);
        }

        public void Dispose() { _dbContext.Dispose(); }
    }
}
