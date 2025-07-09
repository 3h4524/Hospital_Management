using System.Numerics;

namespace Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> FindAll();
        Task<T> FindByID(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task<IEnumerable<T>> Find(Func<T, bool> predicate);
    }
}
