using Microsoft.Extensions.Logging;
using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories;
using StellarStock.Infrastructure.Data.Interfaces;
using StellarStock.Infrastructure.Repositories.Base;

namespace StellarStock.Infrastructure.Repositories
{
    public class EFWarehouseRepository : EFGenericRepository<Warehouse>, IWarehouseRepository
    {
        public EFWarehouseRepository(IUnitOfWork unitOfWork, ILogger<EFWarehouseRepository> logger) : base(unitOfWork, logger) { }

        public async Task<IEnumerable<Warehouse>> GetWarehousesByCityAsync(string cityId)
        {
            try
            {
                var warehouses = await _unitOfWork.GetRepository<Warehouse>()!.GetAllAsync();
                var filteredWarehouses = warehouses!.Where(warehouse => warehouse.Address.City == cityId).ToList();
                _logger.LogInformation($"Retrieved {filteredWarehouses.Count} warehouses by city '{cityId}'");
                return filteredWarehouses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving warehouses by city '{cityId}'");
                throw;
            }
        }

        public async Task<IEnumerable<Warehouse>> GetOpenedWarehouses()
        {
            try
            {
                var warehouses = await _unitOfWork.GetRepository<Warehouse>()!.GetAllAsync();
                var openedWarehouses = warehouses!.Where(warehouse => warehouse.IsOpen).ToList();
                _logger.LogInformation($"Retrieved {openedWarehouses.Count} opened warehouses");
                return openedWarehouses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving opened warehouses");
                throw;
            }
        }

        public async Task<IEnumerable<Warehouse>> GetClosedWarehouses()
        {
            try
            {
                var warehouses = await _unitOfWork.GetRepository<Warehouse>()!.GetAllAsync();
                var closedWarehouses = warehouses!.Where(warehouse => !warehouse.IsOpen).ToList();
                _logger.LogInformation($"Retrieved {closedWarehouses.Count} closed warehouses");
                return closedWarehouses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving closed warehouses");
                throw;
            }
        }

        public async Task<IEnumerable<InventoryItem>> GetWarehouseStockedItems(string warehouseId)
        {
            try
            {
                var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
                var stockedItems = items!.Where(item => item.WarehouseId == warehouseId).ToList();
                _logger.LogInformation($"Retrieved {stockedItems.Count} stocked items for warehouse '{warehouseId}'");
                return stockedItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving stocked items for warehouse '{warehouseId}'");
                throw;
            }
        }
    }
}
