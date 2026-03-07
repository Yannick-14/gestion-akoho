using System.Web.Mvc;
using AkohoAspx.Models;
using System.Threading.Tasks;
using AkohoAspx.Services;

namespace AkohoAspx.Controllers
{
    public class HomeController : Controller
    {
        private readonly SakafoService _sakafoService;

        public HomeController()
        {
            _sakafoService = new SakafoService();
        }

        public async Task<ActionResult> Index()
        {
            await _sakafoService.compterSemaineEcouler(7);
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
