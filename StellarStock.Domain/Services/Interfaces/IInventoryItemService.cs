using StellarStock.Domain.ValueObjects;

namespace StellarStock.Domain.Services.Interfaces
{
    public interface IInventoryItemService
    {
        Task<bool> IsItemAvailableForSaleAsync(string itemId);
        Task<bool> CanItemBeRemovedAsync(string itemId);
        Task<bool> CanItemBeUpdatedAsync(string itemId, string newName, string newDescription, int newPopularityScore, QuantityVO newQuantity, MoneyVO newMoney);
        Task<bool> CanItemBeMovedAsync(string itemId, string newLocationId);
        Task<bool> CanItemBeExpiredAsync(string itemId);
    }
}
