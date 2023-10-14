using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StellarStock.Domain.Repositories.Base;
using StellarStock.Infrastructure.Data.Interfaces;

namespace StellarStock.Infrastructure.Repositories.Base
{
    public class EFGenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger<EFGenericRepository<T>> _logger;

        public EFGenericRepository(IUnitOfWork unitOfWork, ILogger<EFGenericRepository<T>> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<T>?> GetAllAsync()
        {
            try
            {
                var result = await _unitOfWork.Set<T>().ToListAsync();
                _logger.LogInformation($"Retrieved {result.Count} records from {typeof(T).Name}.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting records from {typeof(T).Name}.");
                throw;
            }
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _unitOfWork.Set<T>().FindAsync(id);
                _logger.LogInformation($"Retrieved {result} record from {typeof(T).Name}.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting record from {typeof(T).Name}.");
                throw;
            }
        }

        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                await _unitOfWork.Set<T>().AddAsync(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                    _logger.LogInformation($"Added a new {typeof(T).Name}. Id: {GetEntityId(entity)}");
                else
                    _logger.LogWarning($"Failed to add a new {typeof(T).Name}.");

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while adding a new {typeof(T).Name}.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _unitOfWork.Entry(entity).State = EntityState.Modified;
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                    _logger.LogInformation($"Updated {typeof(T).Name}. Id: {GetEntityId(entity)}");
                else
                    _logger.LogWarning($"Failed to update {typeof(T).Name}.");

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating {typeof(T).Name}.");
                throw;
            }
        }

        public async Task<bool> RemoveAsync(string id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null) return false;

                _unitOfWork.Set<T>().Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                    _logger.LogInformation($"Removed {typeof(T).Name}. Id: {GetEntityId(entity)}");
                else
                    _logger.LogWarning($"Failed to remove {typeof(T).Name}. Id: {id}");

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while removing {typeof(T).Name}.");
                throw;
            }
        }

        private string GetEntityId(T entity)
        {
            var idProperty = entity.GetType().GetProperty("Id");
            return idProperty?.GetValue(entity)?.ToString() ?? "N/A";
        }
    }
}
