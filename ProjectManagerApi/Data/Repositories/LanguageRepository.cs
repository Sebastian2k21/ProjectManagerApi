using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data.Models;
using System.Linq;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public class LanguageRepository : IBaseRepository<Language, int>
    {
        private readonly DataContext context;

        public LanguageRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Language> Add(Language entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Language> Delete(int id)
        {
            var entity = await Get(id);
            context.Languages.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Language>> FindAll(Expression<Func<Language, bool>> expression)
        {
            return await context.Languages.Where(expression).ToListAsync();
        }

        public async Task<Language> FindFirst(Expression<Func<Language, bool>> expression)
        {
            return await context.Languages.FirstOrDefaultAsync(expression);
        }

        public async Task<Language> Get(int id)
        {
            return await FindFirst(x => x.LanguageId == id);
        }

        public async Task<List<Language>> GetAll()
        {
            return await context.Languages.ToListAsync();
        }

        public async Task<Language> Update(Language entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
