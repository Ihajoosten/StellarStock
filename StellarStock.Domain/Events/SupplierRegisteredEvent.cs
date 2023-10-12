using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events
{
    public class SupplierRegisteredEvent
    {
        public Supplier Supplier { get; }

        public SupplierRegisteredEvent(Supplier supplier)
        {
            Supplier = supplier;
        }
    }
}
