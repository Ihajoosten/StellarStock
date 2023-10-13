using Microsoft.EntityFrameworkCore;
using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories;
using StellarStock.Infrastructure.Data.Interfaces;
using StellarStock.Infrastructure.Repositories.Base;

namespace StellarStock.Infrastructure.Repositories
{
    public class EFInventoryItemRepository : EFGenericRepository<InventoryItem>, IInventoryItemRepository
    {
        public EFInventoryItemRepository(IApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<InventoryItem>> GetByCategoryAsync(ItemCategory category)
        {
            return await _context.InventoryItems.Where(item => item.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsByPopularityScoreAsync(int minScore, int maxScore)
        {
            return await _context.InventoryItems.Where(item => item.PopularityScore >= minScore && item.PopularityScore <= maxScore).ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsByWarehouse(string warehouseId)
        {
            return await _context.InventoryItems.Where(item => item.WarehouseId == warehouseId).ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsExpiringSoonAsync(DateTime expirationDate)
        {
            return await _context.InventoryItems.Where(item => item.ValidityPeriod != null && item.ValidityPeriod.EndDate <= expirationDate).ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsInStockAsync()
        {
            return await _context.InventoryItems.Where(item => item.Quantity.Value > 0).ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync(int threshold)
        {
            return await _context.InventoryItems.Where(item => item.Quantity.Value < threshold).ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> GetTopPopularItemsAsync(int count)
        {
            return await _context.InventoryItems.OrderByDescending(item => item.PopularityScore).Take(count).ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> SearchItemsAsync(string keyword)
        {
            return await _context.InventoryItems.Where(item => item.Name.Contains(keyword) || item.Description.Contains(keyword)).ToListAsync();
        }
    }
}
