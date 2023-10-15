using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace StellarStock.Test.Config
{
    public class TestUnitOfWork : IUnitOfWork
    {
        private readonly TestDbContext _context;
        private Dictionary<Type, object> _repositories;
        private IDbContextTransaction _transaction;
        public TestUnitOfWork(TestDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<T>? GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return _repositories[typeof(T)] as IGenericRepository<T>;
            }

            var logger = new Mock<ILogger<EFGenericRepository<T>>>().Object;
            var repository = new EFGenericRepository<T>(unitOfWork: this, logger: logger);
            _repositories[typeof(T)] = repository;
            return repository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
        public DbSet<T> Set<T>() where T : class => _context.Set<T>();
        public EntityEntry<T> Entry<T>(T entity) where T : class => _context.Entry(entity);

        public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();
        public async Task CommitAsync() => await _transaction.CommitAsync();
        public async Task RollbackAsync()
        {
            if (_context != null)
            {
                await _context.Database.RollbackTransactionAsync();
            }
            await _transaction.RollbackAsync();
        }
        public void Dispose() => _transaction?.Dispose();
    }
}
