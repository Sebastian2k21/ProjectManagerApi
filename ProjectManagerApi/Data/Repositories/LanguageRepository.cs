using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data.Models;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public class LanguageRepository : IBaseRepository<Language>
    {
        private readonly DataContext context;

        public LanguageRepository(DataContext context)
        {
            this.context = context;
        }

        public Task<Language> Add(Language entity)
        {
            throw new NotImplementedException();
        }

        public Task<Language> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Language>> FindAll(Expression<Func<Language, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<Language> FindFirst(Expression<Func<Language, bool>> expression)
        {
            return await context.Languages.FirstOrDefaultAsync(expression);
        }

        public async Task<Language> Get(int id)
        {
            return await FindFirst(x => x.LanguageId == id);
        }

        public Task<List<Language>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Language> Update(Language entity)
        {
            throw new NotImplementedException();
        }
    }
}
