using Microsoft.EntityFrameworkCore;
using StellarStock.Domain.Repositories.Base;
using StellarStock.Infrastructure.Data.Interfaces;

namespace StellarStock.Infrastructure.Repositories.Base
{
    public class EFGenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IApplicationDbContext _context;

        public EFGenericRepository(IApplicationDbContext productContext)
        {
            _context = productContext ?? throw new ArgumentNullException(nameof(productContext));
        }

        public async Task<IEnumerable<T>?> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(string id) => await _context.Set<T>().FindAsync(id);

        public async Task<bool> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _context.Set<T>().Remove(entity);
            
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
