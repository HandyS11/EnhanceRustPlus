using EnhanceRustPlus.EfCore.Entities.Interfaces;
using EnhanceRustPlus.EfCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EnhanceRustPlus.EfCore.Implementation
{
    public class UnitOfWork(DbContext context, ILogger<UnitOfWork> logger) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = [];
        private bool _disposed;

        public DbContext DbContext { get; } = context;

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                DbContext.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            DbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            var entries = DbContext.ChangeTracker
                .Entries()
                .Where(e => e is { Entity: IEntity, State: EntityState.Added or EntityState.Modified });

            var nbOfRows = await DbContext.SaveChangesAsync();
            foreach (var entry in DbContext.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

            return nbOfRows;
        }

        public IRepositoryManager<TEntity> GetRepository<TEntity>() where TEntity : class, new()
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new RepositoryManager<TEntity>(DbContext, logger);
            }

            return (IRepositoryManager<TEntity>)_repositories[type];
        }
    }
}
