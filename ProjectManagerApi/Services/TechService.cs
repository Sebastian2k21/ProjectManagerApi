using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Data.Repositories;

namespace ProjectManagerApi.Services
{
    public class TechService : ITechService
    {
        private readonly IBaseRepository<Tech, int> techRepository;

        public TechService(IBaseRepository<Tech, int> techRepository)
        {
            this.techRepository = techRepository;
        }


        public async Task<IEnumerable<Tech>> GetAllTechs()
        {
            return await techRepository.GetAll();
        }

        public async Task<Tech> AddNewTech(Tech tech)
        {
            await techRepository.Add(tech);
            return tech;
        }
    }
}
