
namespace GameZone.Services
{
    public class CategoriesService(ApplicationDbContext context) : ICategoriesService
    {
        private readonly ApplicationDbContext _context = context;
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .OrderBy(c => c.Text)
                    .AsNoTracking()
                    .ToList();
        }
    }
}
