


namespace GameZone.Services
{
    public class GamesService : IGemesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public GamesService(ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }
        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.GameDevices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .ToList();
        }
        public Game? GetById(int id)
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.GameDevices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == id);
        }
        public async Task Create(CreateGameFormViewModel model)
        {
            var coverName = await SaveCover(model.Cover);
            Game game = new()
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                GameDevices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d}).ToList()
            };

            _context.Add(game);
            _context.SaveChanges();
        }
        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = _context.Games
                .Include(g => g.GameDevices)
                .SingleOrDefault(g => g.Id == model.Id);
            if (game is null)
                return null;
            var hasNewCover = model.Cover is not null;
            var oldCover = game.Cover;
            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.GameDevices = model.SelectedDevices.Select(id => new GameDevice { DeviceId = id }).ToList();
            if (hasNewCover)
            {
                var coverName = await SaveCover(model.Cover!);
                game.Cover = coverName;
            }
            var effectedRows = _context.SaveChanges();
            if(effectedRows > 0)
            {
                if(hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }
                return game;
            }
            else
            {
                var newCover = Path.Combine(_imagesPath, game.Cover); 
                File.Delete(newCover);
                return null;
            }
        }
        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);
            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);
            return coverName;
        }

    }
}
