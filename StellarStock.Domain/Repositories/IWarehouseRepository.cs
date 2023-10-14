namespace StellarStock.Domain.Repositories
{
    public interface IWarehouseRepository : IGenericRepository<Warehouse>
    {
        Task<IEnumerable<Warehouse>> GetWarehousesByCityAsync(string city);
        Task<IEnumerable<Warehouse>> GetWarehousesByRegionAsync(string region);
        Task<IEnumerable<Warehouse>> GetOpenedWarehouses();
        Task<IEnumerable<Warehouse>> GetClosedWarehouses();
        Task<IEnumerable<InventoryItem>> GetWarehouseStockedItems(string warehouseId);
    }
}