using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories.Base;

namespace StellarStock.Domain.Repositories
{
    public interface IInventoryItemRepository : IGenericRepository<InventoryItem>
    {
        Task<IEnumerable<InventoryItem>> GetByCategoryAsync(ItemCategory category);
        Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync(int threshold);
        Task<IEnumerable<InventoryItem>> GetItemsExpiringSoonAsync(DateTime expirationDate);
        Task<IEnumerable<InventoryItem>> SearchItemsAsync(string keyword);
        Task<IEnumerable<InventoryItem>> GetItemsByPopularityScoreAsync(int minScore, int maxScore);
        Task<IEnumerable<InventoryItem>> GetTopPopularItemsAsync(int count);
        Task<IEnumerable<InventoryItem>> GetItemsInStockAsync();
        Task<IEnumerable<InventoryItem>> GetItemsByWarehouse(string locationId);
    }
}
