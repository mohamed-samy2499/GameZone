using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
