using System.Linq.Expressions;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
         IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, bool>>? includePredicate = null);
        T? GetOne(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>? includePredicate = null);
        T Create(T entity);

        T Update(T entity);

        bool Delete(T entity);

        int SaveChanges();

    }
}