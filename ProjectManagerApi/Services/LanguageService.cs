using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Data.Repositories;

namespace ProjectManagerApi.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IBaseRepository<Language, int> languageRepository;

        public LanguageService(IBaseRepository<Language, int> languageRepository)
        {
            this.languageRepository = languageRepository;
        }


        public async Task<IEnumerable<Language>> GetAllLanguages()
        {
            return await languageRepository.GetAll();
        }

        public async Task<Language> AddNewLanguage(Language language)
        {
            await languageRepository.Add(language);
            return language;
        }

    }
}
