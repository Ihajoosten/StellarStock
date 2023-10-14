using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories;
using StellarStock.Infrastructure.Data.Interfaces;
using StellarStock.Infrastructure.Repositories.Base;

namespace StellarStock.Infrastructure.Repositories
{
    public class EFWarehouseRepository : EFGenericRepository<Warehouse>, IWarehouseRepository
    {
        public EFWarehouseRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<IEnumerable<Warehouse>> GetWarehousesByCityAsync(string cityId)
        {
            var warehouses = await _unitOfWork.GetRepository<Warehouse>()!.GetAllAsync();
            return warehouses!.Where(warehouse => warehouse.Address.City == cityId).ToList();
        }

        public async Task<IEnumerable<Warehouse>> GetOpenedWarehouses()
        {
            var warehouses = await _unitOfWork.GetRepository<Warehouse>()!.GetAllAsync();
            return warehouses!.Where(warehouse => warehouse.IsOpen).ToList();
        }

        public async Task<IEnumerable<Warehouse>> GetClosedWarehouses()
        {
            var warehouses = await _unitOfWork.GetRepository<Warehouse>()!.GetAllAsync();
            return warehouses!.Where(warehouse => !warehouse.IsOpen).ToList();
        }

        public async Task<IEnumerable<InventoryItem>> GetWarehouseStockedItems(string warehouseId)
        {
            var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
            return items!.Where(item => item.WarehouseId == warehouseId).ToList();
        }
    }
}
