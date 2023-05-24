using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProjectManagerApi.Data.Models;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public class TechRepository : IBaseRepository<Tech, int>
    {
        private readonly DataContext context;

        public TechRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Tech> Add(Tech entity)
        {
            await context.Technologies.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Tech> Delete(int id)
        {
            var entity = await Get(id);
            context.Technologies.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Tech>> FindAll(Expression<Func<Tech, bool>> expression)
        {
            return await context.Technologies.Where(expression).ToListAsync();
        }

        public async Task<Tech> FindFirst(Expression<Func<Tech, bool>> expression)
        {
            return await context.Technologies.FirstOrDefaultAsync(expression);
        }

        public async Task<Tech> Get(int id)
        {
            return await FindFirst(x => x.TechId == id);
        }

        public async Task<List<Tech>> GetAll()
        {
            return await context.Technologies.ToListAsync();
        }

        public async Task<Tech> Update(Tech entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
