using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data.Models;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public class StatusRepository : IBaseRepository<Status, int>
    {
        private readonly DataContext context;

        public StatusRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Status> Add(Status entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Status> Delete(int id)
        {
            var entity = await Get(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Status>> FindAll(Expression<Func<Status, bool>> expression)
        {
            return await context.Statuses.Where(expression).ToListAsync();
        }

        public async Task<Status> FindFirst(Expression<Func<Status, bool>> expression)
        {
            return await context.Statuses.FirstOrDefaultAsync(expression);
        }

        public async Task<Status> Get(int id)
        {
            return await FindFirst(x => x.StatusId == id);
        }

        public async Task<List<Status>> GetAll()
        {
            return await context.Statuses.ToListAsync();
        }

        public async Task<Status> Update(Status entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
