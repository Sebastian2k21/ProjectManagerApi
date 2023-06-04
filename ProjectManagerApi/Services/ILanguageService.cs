using ProjectManagerApi.Data.Models;

namespace ProjectManagerApi.Services
{
    public interface ILanguageService
    {
        Task<IEnumerable<Language>> GetAllLanguages();
        Task<Language> AddNewLanguage(Language language);


    }
}
