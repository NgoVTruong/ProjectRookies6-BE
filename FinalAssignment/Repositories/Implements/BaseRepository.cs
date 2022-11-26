using Data;
using FinalAssignment.Repositories.Implements;
using FinalAssignment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories.Implements
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly FinalAssignmentContext _context;

        public BaseRepository(FinalAssignmentContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public async Task<T> CreateAsync(T entity)
        {
            var result = _dbSet.Add(entity).Entity;

            return await Task.FromResult(result);
        }

        public Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);

            return Task.FromResult(true);
        }

        public async Task<T>? GetOneAsync(Expression<Func<T, bool>>? predicate)
        {
            var result = predicate == null ? _dbSet : _dbSet.Where(predicate);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate)
        {
            var result = predicate == null ? _dbSet : _dbSet.Where(predicate);

            return await result.ToListAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var result = _dbSet.Update(entity).Entity;

            return await Task.FromResult(result);
        }

        public IDatabaseTransaction DatabaseTransaction()
        {
            return new EntityDatabseTransaction(_context);
        }
    }
}