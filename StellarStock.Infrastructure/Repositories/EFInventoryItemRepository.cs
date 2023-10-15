namespace StellarStock.Infrastructure.Repositories
{
    public class EFInventoryItemRepository : EFGenericRepository<InventoryItem>, IInventoryItemRepository
    {
        public EFInventoryItemRepository(IUnitOfWork unitOfWork, ILogger<EFInventoryItemRepository> logger) : base(unitOfWork, logger) { }

        public async Task<IEnumerable<InventoryItem>> GetByCategoryAsync(ItemCategory category)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
                var itemsByCategory = items!.Where(item => item.Category == category).ToList();
                _logger.LogInformation($"Retrieved {itemsByCategory.Count} items by category '{category}'");

                await _unitOfWork.CommitAsync();

                return itemsByCategory;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while retrieving items by category '{category}'");
                throw;
            }
        }
        public async Task<IEnumerable<InventoryItem>> GetItemsByPopularityScoreAsync(int minScore, int maxScore)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
                var itemsByPopularityScore = items!.Where(item => item.PopularityScore >= minScore && item.PopularityScore <= maxScore).ToList();
                _logger.LogInformation($"Retrieved {itemsByPopularityScore.Count} items by popularity score ({minScore}-{maxScore})");

                await _unitOfWork.CommitAsync();

                return itemsByPopularityScore;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while retrieving items by popularity score ({minScore}-{maxScore})");
                throw;
            }
        }
        public async Task<IEnumerable<InventoryItem>> GetItemsByWarehouse(string warehouseId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
                var itemsByWarehouse = items!.Where(item => item.WarehouseId == warehouseId).ToList();
                _logger.LogInformation($"Retrieved {itemsByWarehouse.Count} items by warehouse '{warehouseId}'");

                await _unitOfWork.CommitAsync();

                return itemsByWarehouse;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while retrieving items by warehouse '{warehouseId}'");
                throw;
            }
        }
        public async Task<IEnumerable<InventoryItem>> GetItemsExpiringSoonAsync(DateTime expirationDate)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
                var itemsExpiringSoon = items!
                    .Where(item => item.ValidityPeriod != null && item.ValidityPeriod.EndDate <= expirationDate)
                    .ToList();
                _logger.LogInformation($"Retrieved {itemsExpiringSoon.Count} items expiring soon (before {expirationDate})");

                await _unitOfWork.CommitAsync();

                return itemsExpiringSoon;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while retrieving items expiring soon (before {expirationDate})");
                throw;
            }
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsInStockAsync()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
                var itemsInStock = items!.Where(item => item.Quantity.Value > 0).ToList();
                _logger.LogInformation($"Retrieved {itemsInStock.Count()} items in stock");

                await _unitOfWork.CommitAsync();

                return itemsInStock;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Error while retrieving items in stock");
                throw;
            }
        }


        public async Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync(int threshold)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
                var lowStockItems = items!.Where(item => item.Quantity.Value < threshold).ToList();
                _logger.LogInformation($"Retrieved {lowStockItems.Count} low stock items (threshold: {threshold})");

                await _unitOfWork.CommitAsync();
                return lowStockItems;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while retrieving low stock items (threshold: {threshold})");
                throw;
            }
        }

        public async Task<IEnumerable<InventoryItem>> GetTopPopularItemsAsync(int count)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
                var topPopularItems = items!.OrderByDescending(item => item.PopularityScore).Take(count).ToList();
                _logger.LogInformation($"Retrieved {topPopularItems.Count} top popular items (count: {count})");

                await _unitOfWork.CommitAsync();
                return topPopularItems;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while retrieving top popular items (count: {count})");
                throw;
            }
        }

        public async Task<IEnumerable<InventoryItem>> SearchItemsAsync(string keyword)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var items = await _unitOfWork.GetRepository<InventoryItem>()!.GetAllAsync();
                var searchedItems = items!
                    .Where(item => item.Name.Contains(keyword) || item.Description.Contains(keyword))
                    .ToList();
                _logger.LogInformation($"Retrieved {searchedItems.Count} items by search keyword '{keyword}'");

                await _unitOfWork.CommitAsync();
                return searchedItems;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"Error while retrieving items by search keyword '{keyword}'");
                throw;
            }
        }
    }
}
