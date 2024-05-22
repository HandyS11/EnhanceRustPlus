using EfCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfCore.Implementation
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

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State is EntityState.Added) 
                    ((IEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                else 
                    DbContext.Entry((IEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;

                ((IEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
            }

            var nbOfRows = await DbContext.SaveChangesAsync();
            foreach (var entry in DbContext.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

            return nbOfRows;
        }

        public IRepositoryManager<T> GetRepositoryManager<T>() where T : class, new()
        {
            var type = typeof(IEntity);
            if (_repositories.TryGetValue(type, out var value)) return (IRepositoryManager<T>)value;

            value = new RepositoryManager<IEntity>(DbContext, logger);
            _repositories[type] = value;

            return (IRepositoryManager<T>)value;
        }
    }
}
