using EfCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EfCore.Implementation
{
    public sealed class RepositoryManager<T>(DbContext context, ILogger logger) : IRepositoryManager<T> where T : class
    {
        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                context.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var query = context.Set<T>().AsNoTracking();
            if (navigationPropertyPath != null)
            {
                query = navigationPropertyPath.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.ToListAsync();
        }

        public IQueryable<T> GetAsIQueryable(bool track = false,
            params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var query = track ? context.Set<T>() : context.Set<T>().AsNoTracking();
            if (navigationPropertyPath != null)
            {
                query = navigationPropertyPath.Aggregate(query, (current, include) => current.Include(include));
            }

            return query;
        }

        public async Task<int> CountAsync()
        {
            return await context.Set<T>().AsNoTracking().CountAsync();
        }

        public async Task<T?> AddAsync(T entity)
        {
            try
            {
                var res = await context.Set<T>().AddAsync(entity);

                return res?.Entity;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await AddRangeAsync(entities.ToArray());
        }

        public async Task AddRangeAsync(params T[] entities)
        {
            try
            {
                await context.Set<T>().AddRangeAsync(entities);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        public T Update(T entity)
        {
            try
            {
                context.Entry(entity).State = EntityState.Modified;
                context.Set<T>().Update(entity);

                return entity;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            UpdateRange(entities.ToArray());
        }

        public void UpdateRange(params T[] entities)
        {
            try
            {
                context.Set<T>().UpdateRange(entities);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<T?> DeleteAsync(Guid id)
        {
            try
            {
                var e = await context.Set<T>().FindAsync(id);

                if (e == null)
                {
                    return e;
                }

                context.Set<T>().Remove(e);
                return e;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        public T? Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return entity;
                }

                context.Set<T>().Remove(entity);
                return entity;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                _ = await DeleteAsync(id);
            }
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    return;
                }

                context.Set<T>().RemoveRange(entities);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
