using System.Linq.Expressions;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null);

        T? Get(Expression<Func<T, bool>>? predicate = null);

        T Create(T entity);

        T Update(T entity);

        bool Delete(T entity);

        int SaveChanges();

    }
}