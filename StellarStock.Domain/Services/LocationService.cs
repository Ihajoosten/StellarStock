using StellarStock.Domain.Repositories;
using StellarStock.Domain.Services.Interfaces;
using StellarStock.Domain.ValueObjects;

namespace StellarStock.Domain.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public LocationService(ILocationRepository locationRepository, IInventoryItemRepository inventoryItemRepository)
        {
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
            _inventoryItemRepository = inventoryItemRepository ?? throw new ArgumentNullException(nameof(inventoryItemRepository));
        }

        public async Task<bool> CanLocationBeUpdatedAsync(string locationId, string newStoreName, string newPhone, AddressVO newLocationAddress)
        {
            var location = await _locationRepository.GetByIdAsync(locationId);

            if (location == null) return false;

            if (string.IsNullOrEmpty(newStoreName) || string.IsNullOrEmpty(newPhone) || IsLocationAddressInvalid(newLocationAddress))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CanLocationBeMovedAsync(string locationId, string newAddress, string newCity, string newRegion, string newCountry, string newPostalCode)
        {
            var location = await _locationRepository.GetByIdAsync(locationId);

            if (location == null) return false;

            var newLocationAddress = new AddressVO(newAddress, newPostalCode, newCity, newRegion, newCountry);

            if (!IsLocationAddressInvalid(newLocationAddress)) return false;

            if (!location.IsOpen) return false;

            return true;
        }

        public async Task<bool> CanLocationBeClosedAsync(string locationId, bool isOpen)
        {
            var location = await _locationRepository.GetByIdAsync(locationId);

            if (location == null) return false;

            if (!location.IsOpen) return false;

            return true;
        }

        public async Task<bool> CanLocationBeReopenedAsync(string locationId, bool isOpen)
        {
            var location = await _locationRepository.GetByIdAsync(locationId);

            if (location == null) return false;

            if (location.IsOpen) return false;

            return true;
        }

        public async Task<bool> CanLocationBeDeletedAsync(string locationId)
        {
            var location = await _locationRepository.GetByIdAsync(locationId);

            if (location == null) return false;

            var hasItems = await _inventoryItemRepository.GetItemsByLocation(locationId);

            if (hasItems.Any()) return false;

            return true;
        }

        private static bool IsLocationAddressInvalid(AddressVO address)
        {
            return string.IsNullOrEmpty(address.Country) ||
                   string.IsNullOrEmpty(address.City) ||
                   string.IsNullOrEmpty(address.Region) ||
                   string.IsNullOrEmpty(address.PostalCode) ||
                   string.IsNullOrEmpty(address.Street);
        }
    }
}
