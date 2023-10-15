namespace StellarStock.Infrastructure.Repositories
{
    public class EFWarehouseRepository : EFGenericRepository<Warehouse>, IWarehouseRepository
    {
        public EFWarehouseRepository(IUnitOfWork unitOfWork, ILogger<EFWarehouseRepository> logger) : base(unitOfWork, logger) { }

        public async Task<IEnumerable<Warehouse>> GetWarehousesByCityAsync(string cityId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var warehouses = await _unitOfWork.GetRepository<Warehouse>()!.GetAllAsync();
                var filteredWarehouses = warehouses!.Where(warehouse => warehouse.Address.City == cityId).ToList();
                _logger.LogInformation($"Retrieved {filteredWarehouses.Count} warehouses by city '{cityId}'");

                await _unitOfWork.CommitAsync();
                return filteredWarehouses;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while retrieving warehouses by city '{cityId}'");
                throw;
            }
        }

        public async Task<IEnumerable<Warehouse>> GetWarehousesByRegionAsync(string region)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var warehouses = await _unitOfWork.GetRepository<Warehouse>()!.GetAllAsync();
                var warehousesByRegion = warehouses!.Where(warehouse => warehouse.Address.Region == region).ToList();
                _logger.LogInformation($"Retrieved {warehousesByRegion.Count} warehouses by region '{region}'");

                await _unitOfWork.CommitAsync();
                return warehousesByRegion;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while retrieving warehouses by region '{region}'");
                throw;
            }
        }

        public async Task<IEnumerable<Warehouse>> GetOpenedWarehouses()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var warehouses = await _unitOfWork.GetRepository<Warehouse>()!.GetAllAsync();
                var openedWarehouses = warehouses!.Where(warehouse => warehouse.IsOpen).ToList();
                _logger.LogInformation($"Retrieved {openedWarehouses.Count} opened warehouses");

                await _unitOfWork.CommitAsync();
                return openedWarehouses;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Error while retrieving opened warehouses");
                throw;
            }
        }

        public async Task<IEnumerable<Warehouse>> GetClosedWarehouses()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var warehouses = await _unitOfWork.GetRepository<Warehouse>()!.GetAllAsync();
                var closedWarehouses = warehouses!.Where(warehouse => !warehouse.IsOpen).ToList();
                _logger.LogInformation($"Retrieved {closedWarehouses.Count} closed warehouses");

                await _unitOfWork.CommitAsync();
                return closedWarehouses;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Error while retrieving closed warehouses");
                throw;
            }
        }

        public async Task<IEnumerable<InventoryItem>> GetWarehouseStockedItems(string warehouseId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
                var stockedItems = items!.Where(item => item.WarehouseId == warehouseId).ToList();
                _logger.LogInformation($"Retrieved {stockedItems.Count} stocked items for warehouse '{warehouseId}'");

                await _unitOfWork.CommitAsync();
                return stockedItems;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while retrieving stocked items for warehouse '{warehouseId}'");
                throw;
            }
        }
    }
}
