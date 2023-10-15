namespace StellarStock.Domain.Aggregates
{
    public class InventoryAggregate
    {
        public InventoryItem? InventoryItem { get; private set; }

        public event EventHandler<InventoryItemCreatedEvent> InventoryItemCreated;
        public event EventHandler<InventoryItemQuantityUpdatedEvent> InventoryItemQuantityUpdated;
        public event EventHandler<InventoryItemExpiredEvent> InventoryItemExpired;
        public event EventHandler<InventoryItemMovedEvent> InventoryItemMoved;
        public event EventHandler<InventoryItemRemovedEvent> InventoryItemRemoved;
        public event EventHandler<InventoryItemUpdatedEvent> InventoryItemUpdated;
        public event EventHandler<InventoryItemPopularityUpdatedEvent> InventoryItemPopularityUpdated;
        public event EventHandler<InventoryItemPriceUpdatedEvent> InventoryItemPriceUpdated;
        public event EventHandler<InventoryItemValidityExtendedEvent> InventoryItemValidityExtended;
        public event EventHandler<InventoryItemSoldEvent> InventoryItemSold;
        public event EventHandler<InventoryItemRestockedEvent> InventoryItemRestocked;

        public InventoryAggregate(InventoryItem? inventoryItem)
        {
            InventoryItem = inventoryItem;
            //ValidateAndSetProperties(inventoryItem);
        }

        private void ValidateAndSetProperties(InventoryItem? inventoryItem)
        {
            // Basic validation
            if (inventoryItem == null)
            {
                throw new ArgumentNullException(nameof(inventoryItem));
            }

            // Set properties
            InventoryItem = inventoryItem;
        }

        public void CreateInventoryItem(string name, string description, ItemCategory category, int popularityScore, ProductCodeVO productCode, QuantityVO quantity, MoneyVO money, string warehouseId, string supplierId, DateRangeVO validityPeriod)
        {
            // Validate business rules...
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || popularityScore < 0 || productCode == null
                || quantity == null || money == null || string.IsNullOrEmpty(warehouseId) || string.IsNullOrEmpty(supplierId) || validityPeriod == null)
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("Invalid input for creating an inventory item.");
            }

            // Create the inventory item...
            var inventoryItem = new InventoryItem
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                Category = category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PopularityScore = popularityScore,
                ProductCode = productCode,
                Quantity = quantity,
                Money = money,
                ValidityPeriod = validityPeriod,
                WarehouseId = warehouseId,
                SupplierId = supplierId
            };

            // Raise the events
            OnInventoryItemCreated(inventoryItem);
            OnInventoryItemUpdated(inventoryItem);
        }

        public void UpdateQuantity(int newQuantity)
        {
            // Validate and update quantity...
            if (newQuantity < 0)
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("Quantity must be a non-negative value.");
            }

            var currentQuantity = InventoryItem.Quantity.Value;
            var quantityChange = newQuantity - currentQuantity;
            if (Math.Abs(quantityChange) > 150)
            {
                // Handle business rule violation, throw an exception, or take appropriate action.
                throw new InvalidOperationException("Quantity change exceeds allowed limit.");
            }

            // Update the quantity
            InventoryItem.Quantity = new QuantityVO(newQuantity); // Update the Quantity property with the new value.

            // Raise an event
            OnInventoryItemQuantityUpdated(InventoryItem.Id, newQuantity);
        }

        public void ExpireItem()
        {
            // check if the item is already expired or if there are certain conditions for expiration.
            if (InventoryItem.ValidityPeriod != null && InventoryItem.ValidityPeriod.EndDate <= DateTime.UtcNow)
            {
                // Handle business rule violation, throw an exception, or take appropriate action.
                throw new InvalidOperationException("The item is already expired.");
            }

            // Update the validity period to mark the item as expired.
            InventoryItem.ValidityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow); // Set both start and end dates to the current date/time.

            // Raise an event
            OnInventoryItemExpired(InventoryItem.Id);
        }

        public void MoveItem(string newWarehouseId)
        {
            // Validate and move the item...
            if (string.IsNullOrEmpty(newWarehouseId))
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("New warehouse ID cannot be null or empty.");
            }

            // Check if the item is not expired before moving.
            if (InventoryItem!.ValidityPeriod != null && InventoryItem.ValidityPeriod.EndDate <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot move an expired item.");
            }

            // Update the warehouse ID
            InventoryItem.WarehouseId = newWarehouseId;

            // Raise an event
            OnInventoryItemMoved(InventoryItem.Id, newWarehouseId);
        }

        public void RemoveItem()
        {
            // Check if the item's quantity is greater than zero before removal.
            if (InventoryItem!.Quantity.Value > 0)
            {
                throw new InvalidOperationException("Cannot remove an item with a non-zero quantity.");
            }

            // Check if the item is not expired before removal.
            if (InventoryItem!.ValidityPeriod != null && InventoryItem.ValidityPeriod.EndDate >= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot remove an item that not is expired.");
            }

            // Raise an event
            OnInventoryItemRemoved(InventoryItem.Id);
        }

        public void UpdateItem(string newName, string newDescription, ItemCategory newCategory, ProductCodeVO newProductCode, int newPopularityScore, QuantityVO newQuantity, MoneyVO newMoney)
        {
            // Validate inputs...
            if (string.IsNullOrEmpty(newName))
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("New name cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(newDescription))
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("New description cannot be null or empty.");
            }

            if (!int.IsPositive(newPopularityScore))
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("New popularity score cannot be negative.");
            }

            if (string.IsNullOrEmpty(newProductCode.Code))
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("New product code cannot be null or empty.");
            }

            // Check if the item's quantity is greater than zero before removal.
            if (!int.IsPositive(newQuantity.Value))
            {
                throw new InvalidOperationException("Cannot update an item quantity with a non-zero quantity.");
            }

            // Check if the item is not expired before updating.
            if (InventoryItem.ValidityPeriod != null && InventoryItem.ValidityPeriod.EndDate <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot update an expired item.");
            }

            // Check any other conditions for updating based on your business rules...

            // Update the item information
            InventoryItem.Name = newName;
            InventoryItem.Description = newDescription;
            InventoryItem.Category = newCategory;
            InventoryItem.ProductCode = newProductCode;
            InventoryItem.PopularityScore = newPopularityScore;
            InventoryItem.Quantity = newQuantity;
            InventoryItem.Money = newMoney;
            InventoryItem.UpdatedAt = DateTime.UtcNow;

            // Raise an event
            OnInventoryItemUpdated(InventoryItem);
        }

        public void UpdatePopularityScore(int newPopularityScore)
        {
            // Validate and update popularity score...
            if (newPopularityScore < 0)
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("Popularity score must be a non-negative value.");
            }

            // Check if the item is not expired before updating popularity.
            if (InventoryItem.ValidityPeriod != null && InventoryItem.ValidityPeriod.EndDate <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot update the popularity score of an expired item.");
            }

            // Check if the new popularity score is significantly different from the current score.
            const int PopularityChangeThreshold = 10;
            if (Math.Abs(newPopularityScore - InventoryItem.PopularityScore) > PopularityChangeThreshold)
            {
                throw new InvalidOperationException("Significant changes in popularity score are not allowed.");
            }

            // Update the popularity score
            InventoryItem.PopularityScore = newPopularityScore;

            // Raise an event
            OnInventoryItemPopularityUpdated(InventoryItem.Id, newPopularityScore);
        }

        // Event raising methods
        private void OnInventoryItemCreated(InventoryItem inventoryItem)
        {
            InventoryItemCreated?.Invoke(this, new InventoryItemCreatedEvent(inventoryItem));
        }

        private void OnInventoryItemQuantityUpdated(string inventoryItemId, int newQuantity)
        {
            InventoryItemQuantityUpdated?.Invoke(this, new InventoryItemQuantityUpdatedEvent(inventoryItemId, newQuantity));
        }

        private void OnInventoryItemExpired(string inventoryItemId)
        {
            InventoryItemExpired?.Invoke(this, new InventoryItemExpiredEvent(inventoryItemId));
        }

        private void OnInventoryItemMoved(string inventoryItemId, string newWarehouseId)
        {
            InventoryItemMoved?.Invoke(this, new InventoryItemMovedEvent(inventoryItemId, newWarehouseId));
        }

        private void OnInventoryItemRemoved(string inventoryItemId)
        {
            InventoryItemRemoved?.Invoke(this, new InventoryItemRemovedEvent(inventoryItemId));
        }

        private void OnInventoryItemUpdated(InventoryItem inventoryItem)
        {
            InventoryItemUpdated?.Invoke(this, new InventoryItemUpdatedEvent(inventoryItem));
        }

        private void OnInventoryItemPopularityUpdated(string inventoryItemId, int newPopularityScore)
        {
            InventoryItemPopularityUpdated?.Invoke(this, new InventoryItemPopularityUpdatedEvent(inventoryItemId, newPopularityScore));
        }
    }
}
