using Microsoft.EntityFrameworkCore;
using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories;
using StellarStock.Infrastructure.Data.Interfaces;
using StellarStock.Infrastructure.Repositories.Base;

namespace StellarStock.Infrastructure.Repositories
{
    public class EFWarehouseRepository : EFGenericRepository<Warehouse>, IWarehouseRepository
    {
        public EFWarehouseRepository(IApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Warehouse>> GetWarehousesByCityAsync(string cityId)
        {
            return await _context.Warehouses.Where(warehouse => warehouse.Address.City == cityId).ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetOpenedWarehouses()
        {
            return await _context.Warehouses.Where(warehouse => warehouse.IsOpen).ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetClosedWarehouses()
        {
            return await _context.Warehouses.Where(warehouse => !warehouse.IsOpen).ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> GetWarehouseStockedItems(string warehouseId)
        {
            return await _context.InventoryItems.Where(item => item.WarehouseId == warehouseId).ToListAsync();
        }
    }
}
