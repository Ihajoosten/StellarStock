using StellarStock.Domain.Entities;
using StellarStock.Domain.Events.LocationEvents;
using StellarStock.Domain.Repositories;
using StellarStock.Domain.ValueObjects;

namespace StellarStock.Domain.Aggregates
{
    public class LocationAggregate
    {
        public Location Location { get; private set; }

        public event EventHandler<LocationUpdatedEvent> LocationUpdated;
        public event EventHandler<LocationOpenedEvent> LocationOpened;
        public event EventHandler<LocationClosedEvent> LocationClosed;
        public event EventHandler<LocationDeletedEvent> LocationDeleted;
        public event EventHandler<LocationMovedEvent> LocationMoved;
        public event EventHandler<LocationReopenedEvent> LocationReopened;

        private readonly IInventoryItemRepository _inventoryItemRepository;

        public LocationAggregate(Location? location, IInventoryItemRepository inventoryItemRepository)
        {
            ValidateAndSetProperties(location);
            _inventoryItemRepository = inventoryItemRepository;
        }

        private void ValidateAndSetProperties(Location? location)
        {
            // Basic validation
            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            // Set properties
            Location = location;
        }

        public void CreateLocation(string name, string phone, AddressVO address, bool isOpen)
        {
            // Validate business rules...
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || address == null || IsLocationAddressInvalid(address) || !isOpen)
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("Invalid input for creating an inventory item.");
            }

            // Create the inventory item...
            Location = new Location
            {
                Id = new Guid().ToString(),
                StoreName = name,
                Phone = phone,
                LocationAddress = address,
                IsOpen = isOpen,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            // Raise the events
            OnLocationCreated(Location);
            OnLocationUpdated(Location);
        }

        public void UpdateLocation(string newStoreName, string newPhone, AddressVO newLocationAddress)
        {
            // Validate inputs...
            if (string.IsNullOrEmpty(newStoreName))
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("New name cannot be null or empty.");
            }
            else if (string.IsNullOrEmpty(newPhone))
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("New description cannot be null or empty.");
            }
            else if (IsLocationAddressInvalid(newLocationAddress))
            {
                throw new ArgumentException("New Address properties cannot be null or empty");
            }

            // Update the item information
            Location.StoreName = newStoreName;
            Location.Phone = newPhone;
            Location.LocationAddress = newLocationAddress;
            Location.UpdatedAt = DateTime.Now;

            // Raise an event
            OnLocationUpdated(Location);
        }

        public void MoveLocation(string newAddress, string newCity, string newRegion, string newCountry, string newPostalCode)
        {
            // Basic validation
            if (string.IsNullOrEmpty(newAddress) || string.IsNullOrEmpty(newCity) || string.IsNullOrEmpty(newCountry) || string.IsNullOrEmpty(newPostalCode))
            {
                throw new ArgumentException("New address, city, postal code and country are required for moving the location.");
            }

            Location.LocationAddress.Street = newAddress;
            Location.LocationAddress.City = newCity;
            Location.LocationAddress.Region = newRegion;
            Location.LocationAddress.PostalCode = newPostalCode;
            Location.LocationAddress.Country = newCountry;
            Location.UpdatedAt = DateTime.Now;

            // Raise an event
            OnLocationMoved(Location);
        }

        public void CloseLocation(bool isOpen)
        {
            if (!isOpen) { throw new ArgumentException("Cannot close and already closed store location"); }

            Location.IsOpen = false;
            Location.UpdatedAt = DateTime.Now;

            OnLocationClosed(Location);
        }

        public void ReopenLocation(bool isOpen)
        {
            if (isOpen) { throw new ArgumentException("Cannot open and already opened store location"); }

            Location.IsOpen = false;
            Location.UpdatedAt = DateTime.Now;

            OnLocationReopened(Location);
        }

        public void DeleteLocation()
        {
            // Check if the location is associated with any active inventory items.
            if (IsLocationAssociatedWithActiveItems(_inventoryItemRepository, Location.Id))
            {
                throw new InvalidOperationException("Cannot delete a location that is associated with active inventory items.");
            }

            // Raise an event
            OnLocationDeleted(Location);
        }

        private static bool IsLocationAddressInvalid(AddressVO address)
        {
            return string.IsNullOrEmpty(address.Country) ||
                   string.IsNullOrEmpty(address.City) ||
                   string.IsNullOrEmpty(address.Region) ||
                   string.IsNullOrEmpty(address.PostalCode) ||
                   string.IsNullOrEmpty(address.Street);
        }

        private static bool IsLocationAssociatedWithActiveItems(IInventoryItemRepository inventoryItemRepository, string locationId)
        {
            // Replace this with your actual logic based on your domain model.
            return inventoryItemRepository.GetItemsByLocation(locationId).Result.Any();
        }

        // Event raising methods
        private void OnLocationCreated(Location location)
        {
            LocationOpened?.Invoke(this, new LocationOpenedEvent(location));
        }

        private void OnLocationUpdated(Location location)
        {
            LocationUpdated?.Invoke(this, new LocationUpdatedEvent(location));
        }

        private void OnLocationClosed(Location location)
        {
            LocationClosed?.Invoke(this, new LocationClosedEvent(location.Id));
        }

        private void OnLocationMoved(Location location)
        {
            LocationMoved?.Invoke(this, new LocationMovedEvent(location));
        }

        private void OnLocationReopened(Location location)
        {
            LocationReopened?.Invoke(this, new LocationReopenedEvent(location.Id));
        }

        private void OnLocationDeleted(Location location)
        {
            LocationDeleted?.Invoke(this, new LocationDeletedEvent(location.Id));
        }
    }
}
