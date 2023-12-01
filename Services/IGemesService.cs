namespace GameZone.Services
{
    public interface IGemesService
    {
        Task Create(CreateGameFormViewModel model);
        IEnumerable<Game> GetAll(); 
        Game? GetById(int id);
    }
}
