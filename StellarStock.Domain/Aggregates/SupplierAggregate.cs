
namespace StellarStock.Domain.Aggregates
{
    public class SupplierAggregate
    {
        public Supplier? Supplier { get; private set; }

        public event EventHandler<SupplierActivatedEvent> SupplierActivated;
        public event EventHandler<SupplierDeactivatedEvent> SupplierDeactivated;
        public event EventHandler<SupplierCreatedEvent> SupplierCreated;
        public event EventHandler<SupplierUpdatedEvent> SupplierUpdated;
        public event EventHandler<SupplierDeletedEvent> SupplierDeleted;

        public SupplierAggregate(Supplier? supplier)
        {
            Supplier = supplier;
            //ValidateAndSetProperties(supplier);
        }

        private void ValidateAndSetProperties(Supplier? supplier)
        {
            Supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
        }

        public void CreateSupplier(string name, string phone, string email, AddressVO address, bool isActive, DateRangeVO validityPeriod)
        {
            // Validate business rules...
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || address == null || IsAddressInvalid(address) || !isActive || validityPeriod == null)
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("Invalid input for creating an supplier.");
            }

            // Create the inventory item...
            var supplier = new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            // Raise the events
            OnSupplierCreated(supplier);
            OnSupplierUpdated(supplier);
        }

        public void UpdateSupplier(string newStoreName, string newPhone, string newEmail, AddressVO newAddress)
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
            else if (string.IsNullOrEmpty(newEmail))
            {
                // Handle validation error, throw an exception, or take appropriate action.
                throw new ArgumentException("New email cannot be null or empty.");
            }
            if (IsAddressInvalid(newAddress))
            {
                throw new ArgumentException("New address properties cannot be null or empty");
            }
            // Check if the item is not expired before removal.
            if (Supplier.ValidityPeriod != null && Supplier.ValidityPeriod.EndDate <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot update an expired supplier.");
            }

            // Update the item information
            Supplier.Name = newStoreName;
            Supplier.Phone = newPhone;
            Supplier.ContactEmail = newEmail;
            Supplier.Address = newAddress;
            Supplier.UpdatedAt = DateTime.Now;

            // Raise an event
            OnSupplierUpdated(Supplier);
        }

        public void DeleteSupplier()
        {
            // Raise an event
            OnSupplierDeleted(Supplier);
        }

        public void ActivateSupplier()
        {
            // Check if the supplier is already active or throw an exception if needed
            if (Supplier == null || Supplier.IsActive)
            {
                throw new InvalidOperationException("Supplier is already active or not valid.");
            }

            // Activate the supplier
            Supplier.IsActive = true;
            Supplier.UpdatedAt = DateTime.Now;

            OnSupplierActivated(Supplier);
        }

        public void DeactivateSupplier()
        {
            // Check if the supplier is already inactive or throw an exception if needed
            if (Supplier == null || !Supplier.IsActive)
            {
                throw new InvalidOperationException("Supplier is already inactive or not valid.");
            }

            // Deactivate the supplier
            Supplier.IsActive = false;
            Supplier.UpdatedAt = DateTime.Now;

            OnSupplierDeactivated(Supplier);
        }


        private static bool IsAddressInvalid(AddressVO address)
        {
            return string.IsNullOrEmpty(address.Country) ||
                   string.IsNullOrEmpty(address.City) ||
                   string.IsNullOrEmpty(address.Region) ||
                   string.IsNullOrEmpty(address.PostalCode) ||
                   string.IsNullOrEmpty(address.Street);
        }

        // Event raising methods
        private void OnSupplierCreated(Supplier supplier)
        {
            SupplierCreated?.Invoke(this, new SupplierCreatedEvent(supplier));
        }

        private void OnSupplierUpdated(Supplier supplier)
        {
            SupplierUpdated?.Invoke(this, new SupplierUpdatedEvent(supplier));
        }

        private void OnSupplierDeleted(Supplier supplier)
        {
            SupplierDeleted?.Invoke(this, new SupplierDeletedEvent(supplier));
        }

        private void OnSupplierActivated(Supplier supplier)
        {
            SupplierActivated?.Invoke(this, new SupplierActivatedEvent(supplier));
        }

        private void OnSupplierDeactivated(Supplier supplier)
        {
            SupplierDeactivated?.Invoke(this, new SupplierDeactivatedEvent(supplier));
        }
    }
}