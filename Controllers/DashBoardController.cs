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
            await _dashBoardService.testReste();
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
