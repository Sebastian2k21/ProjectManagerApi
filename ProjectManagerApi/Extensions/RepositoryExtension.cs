using ProjectManagerApi.Data.Repositories;
using ProjectManagerApi.Exceptions;

namespace ProjectManagerApi.Extensions
{
    public static class RepositoryExtension
    {
        public static async Task<List<T>> GetCollectionFromDB<T>(this IBaseRepository<T> repository, List<int> ids, string? errorMessage = null) where T : IEntity<int>
        {
            List<T> items = new List<T>();
            foreach (int id in ids)
            {
                items.Add(await repository.Get(id) ?? throw new InvalidItemIdException(errorMessage));
            }
            return items;
        }
    }
}
