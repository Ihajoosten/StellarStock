using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories.Base;

namespace StellarStock.Domain.Repositories
{
    public interface IWarehouseRepository : IGenericRepository<Warehouse>
    {
        Task<IEnumerable<Warehouse>> GetWarehousesByCityAsync(string cityId);
        Task<IEnumerable<Warehouse>> GetOpenedWarehouses();
        Task<IEnumerable<Warehouse>> GetClosedWarehouses();
        Task<IEnumerable<InventoryItem>> GetWarehouseStockedItems(string warehouseId);
    }
}
