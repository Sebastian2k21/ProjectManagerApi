using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProjectManagerApi.Data.Models;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public class TechRepository : IBaseRepository<Tech>
    {
        private readonly DataContext context;

        public TechRepository(DataContext context)
        {
            this.context = context;
        }

        public Task<Tech> Add(Tech entity)
        {
            throw new NotImplementedException();
        }

        public Task<Tech> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tech>> FindAll(Expression<Func<Tech, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<Tech> FindFirst(Expression<Func<Tech, bool>> expression)
        {
            return await context.Technologies.FirstOrDefaultAsync(expression);
        }

        public async Task<Tech> Get(int id)
        {
            return await FindFirst(x => x.TechId == id);
        }

        public Task<List<Tech>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Tech> Update(Tech entity)
        {
            throw new NotImplementedException();
        }
    }
}
