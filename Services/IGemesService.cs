namespace GameZone.Services
{
    public interface IGemesService
    {
        IEnumerable<Game> GetAll(); 
        Game? GetById(int id);
        Task Create(CreateGameFormViewModel model);
        Task<Game?> Update(EditGameFormViewModel model);
    }
}
