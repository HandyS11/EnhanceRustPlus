using System.Linq.Expressions;

namespace EfCore.Interfaces
{
    public interface IRepositoryManager<T> : IDisposable where T : class
    {
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] navigationPropertyPath);
        IQueryable<T> GetAsIQueryable(bool track = false, params Expression<Func<T, object>>[] navigationPropertyPath);

        Task<T?> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task AddRangeAsync(params T[] entities);

        Task<int> CountAsync();

        T Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void UpdateRange(params T[] entities);

        Task<T?> DeleteAsync(Guid id);
        T? Delete(T entity);
        Task DeleteRangeAsync(IEnumerable<Guid> ids);
        void DeleteRange(IEnumerable<T> entities);
    }
}
