using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data.Models;
using System.Linq;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public class TeamUserRepository : IBaseRepository<TeamUser, (int userId, int teamId, int RoleId)>
    {
        private readonly DataContext context;

        public TeamUserRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<TeamUser> Add(TeamUser entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TeamUser> Delete((int userId, int teamId, int RoleId) id)
        {
            var entity = await Get(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TeamUser>> FindAll(Expression<Func<TeamUser, bool>> expression)
        {
            return await context.TeamUsers.Include(x => x.User).Include(x => x.Role).Where(expression).ToListAsync();
        }

        public async Task<TeamUser> FindFirst(Expression<Func<TeamUser, bool>> expression)
        {
            return await context.TeamUsers.FirstOrDefaultAsync(expression);
        }

        public async Task<TeamUser> Get((int userId, int teamId, int RoleId) id)
        {
            return await FindFirst(x => x.UserId == id.userId && x.RoleId == id.RoleId && x.TeamId == id.teamId);
        }

        public async Task<List<TeamUser>> GetAll()
        {
            return await context.TeamUsers.ToListAsync();
        }

        public async Task<TeamUser> Update(TeamUser entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
