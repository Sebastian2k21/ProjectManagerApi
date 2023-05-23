//using Microsoft.EntityFrameworkCore;
//using ProjectManagerApi.Data.Models;
//using System.Linq.Expressions;

//namespace ProjectManagerApi.Data.Repositories
//{
//    public class Repository<T> : IBaseRepository<T> where T : class, IEntity<int>
//    {
//        private readonly DataContext context;
//        private readonly DbSet<T> dbSet;

//        public Repository(DataContext context)
//        {
//            this.context = context;
//            dbSet = context.Set<T>();
//        }

//        public async Task<T> Add(T entity)
//        {
//            await context.AddAsync(entity);
//            return entity;
//        }

//        public async Task<T> Delete(int id)
//        {
//            var entity = await Get(id);
//            context.Remove(entity);
//            return entity;
//        }

//        public async Task<List<T>> FindAll(Expression<Func<T, bool>> expression)
//        {
//            return await dbSet.Where(expression).ToListAsync();
//        }

//        public async Task<T> FindFirst(Expression<Func<T, bool>> expression)
//        {
//            return await dbSet.FirstOrDefaultAsync(expression);
//        }

//        public async Task<T> Get(int id)
//        {
//            return await dbSet.FirstOrDefaultAsync(x => x.GetId() == id);
//        }

//        public async Task<List<T>> GetAll()
//        {
//            return await dbSet.ToListAsync();
//        }

//        public async Task<T> Update(T entity)
//        {
//            context.Update(entity);
//            await context.SaveChangesAsync();
//            return entity;
//        }
//    }
//}
