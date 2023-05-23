﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Project> FindFirst(Expression<Func<Project, bool>> expression)
        {
            return await context.Projects.FirstOrDefaultAsync(expression);
        }

        public async Task<Project> Get(int id)
        {
            return await FindFirst(x => x.ProjectId == id); 
        }

        public async Task<List<Project>> GetAll()
        {
            return await context.Projects.ToListAsync();
        }

        public async Task<Project> Update(Project entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
