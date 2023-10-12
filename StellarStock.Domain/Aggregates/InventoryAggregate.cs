using StellarStock.Domain.Entities;
using StellarStock.Domain.ValueObjects;

namespace StellarStock.Domain.Aggregates
{
    public class InventoryAggregate
    {
        public InventoryItem? InventoryItem { get; private set; }

        public InventoryAggregate(InventoryItem inventoryItem)
        {
            ValidateAndSetProperties(inventoryItem);
        }

        private void ValidateAndSetProperties(InventoryItem inventoryItem)
        {
            // Basic validation
            if (inventoryItem == null)
            {
                throw new ArgumentNullException(nameof(inventoryItem));
            }

            // Set properties
            InventoryItem = inventoryItem;
        }

        public void UpdateInventoryItemInformation(string newName, string newDescription, ItemCategory newCategory,
        int newPopularityScore, ProductCodeVO newProductCode, QuantityVO newQuantity, MoneyVO newMoney,
        string newLocationId, DateRangeVO newValidityPeriod, string newSupplierId)
        {
            // Check if the inventory item is valid or throw an exception if needed
            if (InventoryItem == null)
            {
                throw new InvalidOperationException("Inventory item is not valid.");
            }

            // Update inventory item information
            InventoryItem.Name = newName;
            InventoryItem.Description = newDescription;
            InventoryItem.Category = newCategory;
            InventoryItem.PopularityScore = newPopularityScore;
            InventoryItem.ProductCode = newProductCode;
            InventoryItem.Quantity = newQuantity;
            InventoryItem.Money = newMoney;
            InventoryItem.LocationId = newLocationId;
            InventoryItem.ValidityPeriod = newValidityPeriod;
            InventoryItem.SupplierId = newSupplierId;
        }
    }
}
