using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Interfaces
{
    public interface IUnitOfWork :IDisposable
    {
        public DbContext DbContext { get; }

        void Save();
        Task<int> SaveAsync();
        IRepositoryManager<T> GetRepositoryManager<T>() where T : class, new();
    }
}
