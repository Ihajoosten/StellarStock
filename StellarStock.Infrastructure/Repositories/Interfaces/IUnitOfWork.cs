using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using StellarStock.Domain.Repositories.Base;

namespace StellarStock.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T>? GetRepository<T>() where T : class;

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
