using System.Threading.Tasks;
using System.Web.Mvc;
using AkohoAspx.Models;
using AkohoAspx.Services;
using AkohoAspx.Services.Results;

namespace AkohoAspx.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly DashBoardService _dashBoardService;
        public DashBoardController()
        {
            _dashBoardService = new DashBoardService();
        }
        public async Task<ActionResult> Index()
        {
            var data = await _dashBoardService.GetDashboardDataAsync();
            return View(data);
        }

        public ActionResult MakaAtody(int lotId, int raceId)
        {
            ViewBag.LotId = lotId;
            ViewBag.RaceId = raceId;
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = string.Empty });
        }
    }
}
