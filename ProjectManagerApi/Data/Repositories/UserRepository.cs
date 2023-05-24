using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data.Models;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public class UserRepository : IBaseRepository<User, int>
    {
        private readonly DataContext context;

        public UserRepository(DataContext context)
        {
            this.context = context;
        }
        public async Task<User> Add(User entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> Delete(int id)
        {
            var entity = await Get(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<User>> FindAll(Expression<Func<User, bool>> expression)
        {
            return await context.Users.Where(expression).ToListAsync();
        }

        public async Task<User> FindFirst(Expression<Func<User, bool>> expression)
        {
            return await context.Users.FirstOrDefaultAsync(expression);
        }

        public async Task<User> Get(int id)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> Update(User entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
