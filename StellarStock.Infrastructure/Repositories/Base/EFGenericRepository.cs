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
                await _unitOfWork.BeginTransactionAsync();

                var result = await _unitOfWork.Set<T>().ToListAsync();
                _logger.LogInformation($"Retrieved {result.Count} records from {typeof(T).Name}.");

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while getting records from {typeof(T).Name}.");
                throw;
            }
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await _unitOfWork.Set<T>().FindAsync(id);
                _logger.LogInformation($"Retrieved {result} record from {typeof(T).Name}.");

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while getting record from {typeof(T).Name}.");
                throw;
            }
        }
        public async Task<bool> AddAsync(T entity)
        {
            var entityId = GetEntityId(entity);
            if (entityId == "N/A") return false;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    _logger.LogInformation($"Adding a new {typeof(T).Name}. Id: {entityId}");

                    await _unitOfWork.Set<T>().AddAsync(entity);
                    var result = await _unitOfWork.SaveChangesAsync();

                    if (result > 0)
                    {
                        _logger.LogInformation($"Added a new {typeof(T).Name}. Id: {entityId}");
                        await _unitOfWork.CommitAsync();
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to add a new {typeof(T).Name}. Id: {entityId}");
                    }

                    return result > 0;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error while adding a new {typeof(T).Name}. Id: {entityId}");
                    await _unitOfWork.RollbackAsync();
                    throw new Exception($"Error while adding new Entity :: ${ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if BeginTransactionAsync fails
                _logger.LogError(ex, $"Error beginning transaction for adding a new {typeof(T).Name}. Id: {entityId}");
                await _unitOfWork.RollbackAsync();
                throw new Exception($"Error beginning transaction for adding a new Entity :: ${ex.Message}");
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var entityId = GetEntityId(entity);
            if (entityId == "N/A") return false;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    _unitOfWork.Entry(entity).State = EntityState.Modified;
                    var result = await _unitOfWork.SaveChangesAsync();

                    if (result > 0)
                    {
                        _logger.LogInformation($"Updated {typeof(T).Name}. Id: {entityId}");
                        await _unitOfWork.CommitAsync();
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to update {typeof(T).Name}.");
                        await _unitOfWork.RollbackAsync();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error while updating a new {typeof(T).Name}.");
                    await _unitOfWork.RollbackAsync();
                    throw new Exception($"Error while updating new Entity :: ${ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if BeginTransactionAsync fails
                _logger.LogError(ex, $"Error beginning transaction for updating a new {typeof(T).Name}.");
                await _unitOfWork.RollbackAsync();
                throw new Exception($"Error beginning transaction for updating a new Entity :: ${ex.Message}");
            }
        }

        public async Task<bool> RemoveAsync(string id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = await GetByIdAsync(id);
                if (entity == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                _unitOfWork.Set<T>().Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation($"Removed {typeof(T).Name}. Id: {GetEntityId(entity)}");
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    _logger.LogWarning($"Failed to remove {typeof(T).Name}. Id: {id}");
                    await _unitOfWork.RollbackAsync();
                }

                return result > 0;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
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
