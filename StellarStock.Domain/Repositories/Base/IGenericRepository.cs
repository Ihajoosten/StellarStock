namespace StellarStock.Domain.Repositories.Base
{
    public interface IGenericRepository<T>
    {
        Task<T?> GetByIdAsync(string id);
        Task<IEnumerable<T>?> GetAllAsync();
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> RemoveAsync(string id);
    }
}
