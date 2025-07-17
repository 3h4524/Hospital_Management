using System.Linq.Expressions;
using System.Numerics;

namespace Repository
{
    public interface GenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> FindAll();
        Task<T?> FindByID(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
}
