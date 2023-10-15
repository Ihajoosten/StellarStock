namespace StellarStock.Domain.Aggregates
{
    public class WarehouseAggregate
    {
        public Warehouse? Warehouse { get; private set; }

        public event EventHandler<WarehouseUpdatedEvent> WarehouseUpdated;
        public event EventHandler<WarehouseOpenedEvent> WarehouseOpened;
        public event EventHandler<WarehouseClosedEvent> WarehouseClosed;
        public event EventHandler<WarehouseDeletedEvent> WarehouseDeleted;
        public event EventHandler<WarehouseMovedEvent> WarehouseMoved;
        public event EventHandler<WarehouseReopenedEvent> WarehouseReopened;

        private readonly IInventoryItemRepository _inventoryItemRepository;

        public WarehouseAggregate(Warehouse? warehouse)
        {
            ValidateAndSetProperties(warehouse);
        }

        private void ValidateAndSetProperties(Warehouse? warehouse)
        {
            // Basic validation
            if (warehouse == null)
            {
                throw new ArgumentNullException(nameof(warehouse));
            }

            // Set properties
            Warehouse = warehouse;
        }

        public void CreateWarehouse(string name, string phone, AddressVO address, bool isOpen)
        {
            // Validate business rules...
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || address == null || IsAddressInvalid(address) || !isOpen)
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("Invalid input for creating an inventory item.");
            }

            // Create the inventory item...
            Warehouse = new Warehouse
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Phone = phone,
                Address = address,
                IsOpen = isOpen,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            // Raise the events
            OnWarehouseCreated(Warehouse);
            OnWarehouseUpdated(Warehouse);
        }

        public void UpdateWarehouse(string newName, string newPhone, AddressVO newAddress)
        {
            // Validate inputs...
            if (string.IsNullOrEmpty(newName))
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("New name cannot be null or empty.");
            }
            else if (string.IsNullOrEmpty(newPhone))
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("New description cannot be null or empty.");
            }
            else if (IsAddressInvalid(newAddress))
            {
                throw new ArgumentException("New address properties cannot be null or empty");
            }

            // Update the item information
            Warehouse.Name = newName;
            Warehouse.Phone = newPhone;
            Warehouse.Address = newAddress;
            Warehouse.UpdatedAt = DateTime.Now;

            // Raise an event
            OnWarehouseUpdated(Warehouse);
        }

        public void MoveWarehouse(string newAddress, string newCity, string newRegion, string newCountry, string newPostalCode)
        {
            // Basic validation
            if (string.IsNullOrEmpty(newAddress) || string.IsNullOrEmpty(newCity) || string.IsNullOrEmpty(newRegion) || string.IsNullOrEmpty(newCountry) || string.IsNullOrEmpty(newPostalCode))
            {
                throw new ArgumentException("New address, city, postal code, region and country are required for moving the Warehouse.");
            }

            Warehouse.Address.Street = newAddress;
            Warehouse.Address.City = newCity;
            Warehouse.Address.Region = newRegion;
            Warehouse.Address.PostalCode = newPostalCode;
            Warehouse.Address.Country = newCountry;
            Warehouse.UpdatedAt = DateTime.Now;

            // Raise an event
            OnWarehouseMoved(Warehouse);
        }

        public void CloseWarehouse(bool isOpen)
        {
            if (!isOpen) { throw new ArgumentException("Cannot close an already closed Warehouse"); }

            Warehouse.IsOpen = false;
            Warehouse.UpdatedAt = DateTime.Now;

            OnWarehouseClosed(Warehouse);
        }

        public void ReopenWarehouse(bool isOpen)
        {
            if (isOpen) { throw new ArgumentException("Cannot open an already opened Warehouse"); }

            Warehouse.IsOpen = false;
            Warehouse.UpdatedAt = DateTime.Now;

            OnWarehouseReopened(Warehouse);
        }

        public void DeleteWarehouse()
        {
            // Check if the Warehouse is associated with any active inventory items.
            if (IsWarehouseAssociatedWithActiveItems(_inventoryItemRepository, Warehouse.Id))
            {
                throw new InvalidOperationException("Cannot delete a Warehouse that is associated with active inventory items.");
            }

            // Raise an event
            OnWarehouseDeleted(Warehouse);
        }

        private static bool IsAddressInvalid(AddressVO address)
        {
            return string.IsNullOrEmpty(address.Country) ||
                   string.IsNullOrEmpty(address.City) ||
                   string.IsNullOrEmpty(address.Region) ||
                   string.IsNullOrEmpty(address.PostalCode) ||
                   string.IsNullOrEmpty(address.Street);
        }

        private static bool IsWarehouseAssociatedWithActiveItems(IInventoryItemRepository inventoryItemRepository, string WarehouseId)
        {
            // Replace this with your actual logic based on your domain model.
            return inventoryItemRepository.GetItemsByWarehouse(WarehouseId).Result.Any();
        }

        // Event raising methods
        private void OnWarehouseCreated(Warehouse Warehouse)
        {
            WarehouseOpened?.Invoke(this, new WarehouseOpenedEvent(Warehouse));
        }

        private void OnWarehouseUpdated(Warehouse Warehouse)
        {
            WarehouseUpdated?.Invoke(this, new WarehouseUpdatedEvent(Warehouse));
        }

        private void OnWarehouseClosed(Warehouse Warehouse)
        {
            WarehouseClosed?.Invoke(this, new WarehouseClosedEvent(Warehouse.Id));
        }

        private void OnWarehouseMoved(Warehouse Warehouse)
        {
            WarehouseMoved?.Invoke(this, new WarehouseMovedEvent(Warehouse));
        }

        private void OnWarehouseReopened(Warehouse Warehouse)
        {
            WarehouseReopened?.Invoke(this, new WarehouseReopenedEvent(Warehouse.Id));
        }

        private void OnWarehouseDeleted(Warehouse Warehouse)
        {
            WarehouseDeleted?.Invoke(this, new WarehouseDeletedEvent(Warehouse.Id));
        }
    }
}
