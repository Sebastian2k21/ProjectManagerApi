using ProjectManagerApi.Data.Models;

namespace ProjectManagerApi.Services
{
    public interface ITechService
    {
        Task<IEnumerable<Tech>> GetAllTechs();
        Task<Tech> AddNewTech(Tech tech);
    }
}
