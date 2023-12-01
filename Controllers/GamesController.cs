

using GameZone.Services;

namespace GameZone.Controllers
{
    public class GamesController(ApplicationDbContext context,ICategoriesService categoriesService
        , IDevicesService devicesService, IGemesService gemesService) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ICategoriesService _categoriesService = categoriesService;
        private readonly IDevicesService _devicesService = devicesService;
        private readonly IGemesService _gamesService = gemesService;

        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }
        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);
            if (game is null)
                return NotFound();
            return View(game);
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

            await _gamesService.Create(model);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var game = _gamesService.GetById(id); 
            if (game is null)
                return NotFound();
            EditGameFormViewModel editGameFormViewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CurrentCover = game.Cover,
                CategoryId = game.CategoryId,
                SelectedDevices = game.GameDevices.Select( gd => gd.DeviceId).ToList(),
                Devices = _devicesService.GetSelectList(),
                Categories = _categoriesService.GetSelectList()
            };
            return View(editGameFormViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesService.GetSelectList();
                return View(model);
            }

            var game = await _gamesService.Update(model);
            if (game is null)
                return BadRequest();
            return RedirectToAction(nameof(Index));
        }
    }
}
