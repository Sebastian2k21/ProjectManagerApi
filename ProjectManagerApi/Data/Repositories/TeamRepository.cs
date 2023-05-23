using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data.Models;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public class TeamRepository : IBaseRepository<Team>
    {
        private readonly DataContext context;

        public TeamRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Team> Add(Team entity)
        {
            await context.AddAsync(entity);
            return entity;
        }

        public async Task<Team> Delete(int id)
        {
            var entity = await Get(id);
            context.Remove(entity);
            return entity;
        }

        public async Task<List<Team>> FindAll(Expression<Func<Team, bool>> expression)
        {
            return await context.Teams.Where(expression).ToListAsync();
        }

        public async Task<Team> FindFirst(Expression<Func<Team, bool>> expression)
        {
            return await context.Teams.FirstOrDefaultAsync(expression);
        }

        public async Task<Team> Get(int id)
        {
            return await FindFirst(x => x.TeamId == id);
        }

        public async Task<List<Team>> GetAll()
        {
            return await context.Teams.ToListAsync();
        }

        public async Task<Team> Update(Team entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
