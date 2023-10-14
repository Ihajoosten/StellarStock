using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories;
using StellarStock.Infrastructure.Data.Interfaces;
using StellarStock.Infrastructure.Repositories.Base;

namespace StellarStock.Infrastructure.Repositories
{
    public class EFInventoryItemRepository : EFGenericRepository<InventoryItem>, IInventoryItemRepository
    {
        public EFInventoryItemRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<IEnumerable<InventoryItem>> GetByCategoryAsync(ItemCategory category)
        {
            var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
            return items!.Where(item => item.Category == category).ToList();
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsByPopularityScoreAsync(int minScore, int maxScore)
        {
            var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
            return items!.Where(item => item.PopularityScore >= minScore && item.PopularityScore <= maxScore).ToList();
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsByWarehouse(string warehouseId)
        {
            var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
            return items!.Where(item => item.WarehouseId == warehouseId).ToList();
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsExpiringSoonAsync(DateTime expirationDate)
        {
            var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
            return items!.Where(item => item.ValidityPeriod != null && item.ValidityPeriod.EndDate <= expirationDate).ToList();
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsInStockAsync()
        {
            var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
            return items!.Where(item => item.Quantity.Value > 0).ToList();
        }

        public async Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync(int threshold)
        {
            var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
            return items!.Where(item => item.Quantity.Value < threshold).ToList();
        }

        public async Task<IEnumerable<InventoryItem>> GetTopPopularItemsAsync(int count)
        {
            var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
            return items!.OrderByDescending(item => item.PopularityScore).Take(count).ToList();
        }

        public async Task<IEnumerable<InventoryItem>> SearchItemsAsync(string keyword)
        {
            var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
            return items!.Where(item => item.Name.Contains(keyword) || item.Description.Contains(keyword)).ToList();
        }
    }
}
