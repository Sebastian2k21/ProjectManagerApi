namespace ProjectManagerApi.Data.Repositories
{
    using global::ProjectManagerApi.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using System.Linq;
    using System.Linq.Expressions;

    namespace ProjectManagerApi.Data.Repositories
    {
        public class RoleRepository : IBaseRepository<Role, int>
        {
            private readonly DataContext context;

            public RoleRepository(DataContext context)
            {
                this.context = context;
            }

            public async Task<Role> Add(Role entity)
            {
                await context.AddAsync(entity);
                await context.SaveChangesAsync();
                return entity;
            }

            public async Task<Role> Delete(int id)
            {
                var entity = await Get(id);
                context.Remove(entity);
                await context.SaveChangesAsync();
                return entity;
            }

            public async Task<List<Role>> FindAll(Expression<Func<Role, bool>> expression)
            {
                return await context.Roles.Where(expression).ToListAsync();
            }

            public async Task<Role> FindFirst(Expression<Func<Role, bool>> expression)
            {
                return await context.Roles.FirstOrDefaultAsync(expression);
            }

            public async Task<Role> Get(int id)
            {
                return await FindFirst(x => x.RoleId == id);
            }

            public async Task<List<Role>> GetAll()
            {
                return await context.Roles.ToListAsync();
            }

            public async Task<Role> Update(Role entity)
            {
                context.Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }
    }

}
