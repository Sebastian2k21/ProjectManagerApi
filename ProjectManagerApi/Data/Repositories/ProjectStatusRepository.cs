using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data.Models;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public class ProjectStatusRepository : IBaseRepository<ProjectStatus, int>
    {
        private readonly DataContext context;

        public ProjectStatusRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<ProjectStatus> Add(ProjectStatus entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<ProjectStatus> Delete(int id)
        {
            var entity = await Get(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<ProjectStatus>> FindAll(Expression<Func<ProjectStatus, bool>> expression)
        {
            return await context.ProjectStatuses.Where(expression).ToListAsync();
        }

        public async Task<ProjectStatus> FindFirst(Expression<Func<ProjectStatus, bool>> expression)
        {
            return await context.ProjectStatuses.FirstOrDefaultAsync(expression);
        }

        public async Task<ProjectStatus> Get(int id)
        {
            return await FindFirst(x => x.ProjectStatusId == id);
        }

        public async Task<List<ProjectStatus>> GetAll()
        {
            return await context.ProjectStatuses.ToListAsync();
        }

        public async Task<ProjectStatus> Update(ProjectStatus entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
