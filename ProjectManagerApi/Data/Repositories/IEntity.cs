namespace ProjectManagerApi.Data.Repositories
{
    public interface IEntity<T>
    {
        public T GetId();
    }
}
