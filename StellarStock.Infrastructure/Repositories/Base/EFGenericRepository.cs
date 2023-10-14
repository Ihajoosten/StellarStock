using Microsoft.EntityFrameworkCore;
using StellarStock.Domain.Repositories.Base;
using StellarStock.Infrastructure.Data.Interfaces;

namespace StellarStock.Infrastructure.Repositories.Base
{
    public class EFGenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public EFGenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<T>?> GetAllAsync() => await _unitOfWork.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(string id) => await _unitOfWork.Set<T>().FindAsync(id);

        public async Task<bool> AddAsync(T entity)
        {
            await _unitOfWork.Set<T>().AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _unitOfWork.Entry(entity).State = EntityState.Modified;
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.Set<T>().Remove(entity);
            
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
    }
}
