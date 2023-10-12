using StellarStock.Domain.Entities;
using StellarStock.Domain.ValueObjects;

namespace StellarStock.Domain.Aggregates
{
    public class SupplierAggregate
    {
        public Supplier? Supplier { get; private set; }

        public SupplierAggregate(Supplier supplier)
        {
            ValidateAndSetProperties(supplier);
        }

        private void ValidateAndSetProperties(Supplier supplier)
        {
            Supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
        }

        public void UpdateSupplierInformation(string newName, string newPhone, string newContactEmail, AddressVO newLocationAddress)
        {
            // Check if the supplier is valid or throw an exception if needed
            if (Supplier == null)
            {
                throw new InvalidOperationException("Supplier is not valid.");
            }

            // Update supplier information
            Supplier.Name = newName;
            Supplier.Phone = newPhone;
            Supplier.ContactEmail = newContactEmail;
            Supplier.LocationAddress = newLocationAddress;

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
        }
    }
}
