using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories.Base;

namespace StellarStock.Domain.Repositories
{
    public interface IWarehouseRepository : IGenericRepository<Warehouse>
    {
        Task<IEnumerable<Warehouse>> GetWarehousesByCityAsync(string cityId);
    }
}
