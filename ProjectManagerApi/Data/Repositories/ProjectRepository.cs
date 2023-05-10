using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data.Models;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public class ProjectRepository : IBaseRepository<Project>
    {
        private readonly DataContext context;

        public ProjectRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Project> Add(Project entity)
        {
            await context.AddRangeAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Project> Delete(int id)
        {
            var entity = await Get(id);
            if (entity != null)
            {
                context.Remove(entity);
                await context.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<List<Project>> FindAll(Expression<Func<Project, bool>> expression)
        {
            return await context.Projects.Where(expression).ToListAsync();
        }

        public Task<Project> FindFirst(Expression<Func<User, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Project> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Project> Update(Project entity)
        {
            throw new NotImplementedException();
        }
    }
}
