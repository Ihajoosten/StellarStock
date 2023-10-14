namespace StellarStock.Domain.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _WarehouseRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public WarehouseService(IWarehouseRepository WarehouseRepository, IInventoryItemRepository inventoryItemRepository)
        {
            _WarehouseRepository = WarehouseRepository ?? throw new ArgumentNullException(nameof(WarehouseRepository));
            _inventoryItemRepository = inventoryItemRepository ?? throw new ArgumentNullException(nameof(inventoryItemRepository));
        }

        public async Task<bool> CanWarehouseBeUpdatedAsync(string WarehouseId, string newStoreName, string newPhone, AddressVO newWarehouseAddress)
        {
            var Warehouse = await _WarehouseRepository.GetByIdAsync(WarehouseId);

            if (Warehouse == null) return false;

            if (string.IsNullOrEmpty(newStoreName) || string.IsNullOrEmpty(newPhone) || IsWarehouseAddressInvalid(newWarehouseAddress))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CanWarehouseBeMovedAsync(string WarehouseId, string newAddress, string newCity, string newRegion, string newCountry, string newPostalCode)
        {
            var Warehouse = await _WarehouseRepository.GetByIdAsync(WarehouseId);

            if (Warehouse == null) return false;

            var newWarehouseAddress = new AddressVO(newAddress, newPostalCode, newCity, newRegion, newCountry);

            if (!IsWarehouseAddressInvalid(newWarehouseAddress)) return false;

            if (!Warehouse.IsOpen) return false;

            return true;
        }

        public async Task<bool> CanWarehouseBeClosedAsync(string WarehouseId, bool isOpen)
        {
            var Warehouse = await _WarehouseRepository.GetByIdAsync(WarehouseId);

            if (Warehouse == null) return false;

            if (!Warehouse.IsOpen) return false;

            return true;
        }

        public async Task<bool> CanWarehouseBeReopenedAsync(string WarehouseId, bool isOpen)
        {
            var Warehouse = await _WarehouseRepository.GetByIdAsync(WarehouseId);

            if (Warehouse == null) return false;

            if (Warehouse.IsOpen) return false;

            return true;
        }

        public async Task<bool> CanWarehouseBeDeletedAsync(string WarehouseId)
        {
            var Warehouse = await _WarehouseRepository.GetByIdAsync(WarehouseId);

            if (Warehouse == null) return false;

            var hasItems = await _inventoryItemRepository.GetItemsByWarehouse(WarehouseId);

            if (hasItems.Any()) return false;

            return true;
        }

        private static bool IsWarehouseAddressInvalid(AddressVO address)
        {
            return string.IsNullOrEmpty(address.Country) ||
                   string.IsNullOrEmpty(address.City) ||
                   string.IsNullOrEmpty(address.Region) ||
                   string.IsNullOrEmpty(address.PostalCode) ||
                   string.IsNullOrEmpty(address.Street);
        }
    }
}
