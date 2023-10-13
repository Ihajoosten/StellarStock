using StellarStock.Domain.Repositories;
using StellarStock.Domain.Services.Interfaces;
using StellarStock.Domain.ValueObjects;

namespace StellarStock.Domain.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly ILocationRepository _locationRepository;

        public InventoryItemService(IInventoryItemRepository inventoryItemRepository, ILocationRepository locationRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _locationRepository = locationRepository;
        }

        public async Task<bool> IsItemAvailableForSaleAsync(string itemId)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(itemId);
            return item.Quantity.Value > 0 && item.ValidityPeriod != null && item.ValidityPeriod.EndDate > DateTime.UtcNow;
        }

        public async Task<bool> CanItemBeRemovedAsync(string itemId)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(itemId);
            return item.Quantity.Value == 0 && (item.ValidityPeriod == null || item.ValidityPeriod.EndDate <= DateTime.UtcNow);
        }

        public async Task<bool> CanItemBeUpdatedAsync(string itemId, string newName, string newDescription, int newPopularityScore, QuantityVO newQuantity, MoneyVO newMoney)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(itemId);

            // Business Rule: Item should not be expired
            if (item.ValidityPeriod != null && item.ValidityPeriod.EndDate <= DateTime.UtcNow) return false;

            // Business Rule: Check if the new name or description are null or empty
            if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newDescription)) return false;

            // Business Rule: Check if the new popularity score is within a reasonable range
            if (newPopularityScore < 0 || newPopularityScore > 100) return false;

            // Business Rule: Check if the new Quantity is not negative
            if (newQuantity.Value < 0) return false;

            // Business Rule: Check if the new quantity is within a reasonable range
            var currentQuantity = item.Quantity.Value;
            var quantityChange = newQuantity.Value - currentQuantity;
            if (Math.Abs(quantityChange) > 150) return false;

            return true;
        }

        public async Task<bool> CanItemBeExpiredAsync(string itemId)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(itemId);

            // Business Rule: Item should not be already expired
            if (item.ValidityPeriod != null && item.ValidityPeriod.EndDate <= DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CanItemBeMovedAsync(string itemId, string newLocationId)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(itemId);

            // Business Rule: Item should not be expired
            if (item.ValidityPeriod != null && item.ValidityPeriod.EndDate <= DateTime.UtcNow) return false;


            // Business Rule: Check if the new location is valid
            var isValid = await IsValidLocation(newLocationId);
            if (string.IsNullOrEmpty(newLocationId) || !isValid) return false;

            return true;
        }

        private async Task<bool> IsValidLocation(string locationId)
        {
            return !string.IsNullOrEmpty(locationId) && await LocationExists(locationId);
        }

        private async Task<bool> LocationExists(string locationId)
        {
            try
            {
                var location = await _locationRepository.GetByIdAsync(locationId);
                return location != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
