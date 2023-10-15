namespace StellarStock.Domain.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<bool> CanSupplierBeUpdatedAsync(string supplierId, string newName, string newPhone, string newEmail, AddressVO newAddress)
        {
            var supplier = await _supplierRepository.GetByIdAsync(supplierId);

            // Business Rule: Check if Supplier exist
            if (supplier == null)
            {
                return false;
            }

            // Business Rule: Check if new name, phone, email and address are not null or empty
            if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newPhone) || string.IsNullOrEmpty(newEmail) || IsSupplierAddressInvalid(newAddress))
            {
                return false;
            }

            // Business Rule: check if supplier is not expired
            if (supplier.ValidityPeriod != null && supplier.ValidityPeriod.EndDate <= DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CanSupplierBeDeletedAsync(string supplierId)
        {
            var supplier = await _supplierRepository.GetByIdAsync(supplierId);

            // Business Rule: Check if Supplier exist
            if (supplier == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CanSupplierBeActivatedAsync(string supplierId)
        {
            var supplier = await _supplierRepository.GetByIdAsync(supplierId);

            // Business Rule: Check if Supplier exist
            if (supplier == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CanSupplierBeDeactivatedAsync(string supplierId)
        {
            var supplier = await _supplierRepository.GetByIdAsync(supplierId);

            // Business Rule: Check if Supplier exist
            if (supplier == null)
            {
                return false;
            }

            return true;
        }

        private static bool IsSupplierAddressInvalid(AddressVO address)
        {
            return string.IsNullOrEmpty(address.Country) ||
                   string.IsNullOrEmpty(address.City) ||
                   string.IsNullOrEmpty(address.Region) ||
                   string.IsNullOrEmpty(address.PostalCode) ||
                   string.IsNullOrEmpty(address.Street);
        }
    }
}
