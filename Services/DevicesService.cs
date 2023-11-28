
namespace GameZone.Services
{
    public class DevicesService(ApplicationDbContext context) : IDevicesService
    {
        private readonly ApplicationDbContext _context = context;
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _context.Devices
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .OrderBy(c => c.Text)
                    .AsNoTracking()
                    .ToList();
        }
    }
}
