using Microsoft.AspNetCore.Mvc;

namespace ActivityClub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Events()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
