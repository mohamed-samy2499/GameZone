

using GameZone.Services;

namespace GameZone.Controllers
{
    public class GamesController(ApplicationDbContext context,ICategoriesService categoriesService
        , IDevicesService devicesService, IGemesService gemesServices) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ICategoriesService _categoriesService = categoriesService;
        private readonly IDevicesService _devicesService = devicesService;
        private readonly IGemesService _gemesServices = gemesServices;

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
        
            CreateGameFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList()

            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateGameFormViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesService.GetSelectList();
                return View(model);
            }

            await _gemesServices.Create(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
