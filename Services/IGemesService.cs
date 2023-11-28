namespace GameZone.Services
{
    public interface IGemesService
    {
        Task Create(CreateGameFormViewModel model);
    }
}
