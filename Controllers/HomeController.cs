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
            var depense = await _sakafoService.getDepenseAlimentActuelleLotEnGramme(11);
            System.Console.WriteLine($"[HOME] depense trouvés: {depense}");

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
