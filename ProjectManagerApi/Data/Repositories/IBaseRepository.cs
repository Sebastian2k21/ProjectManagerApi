using ProjectManagerApi.Data.Models;
using System.Linq.Expressions;

namespace ProjectManagerApi.Data.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<List<T>> GetAll();
        Task<List<T>> FindAll(Expression<Func<T, bool>> expression);
        Task<T> FindFirst(Expression<Func<T, bool>> expression);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
    }
}
