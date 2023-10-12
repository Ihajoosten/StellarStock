using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events
{
    public class SupplierDeactivatedEvent
    {
        public Supplier Supplier { get; }

        public SupplierDeactivatedEvent(Supplier supplier)
        {
            Supplier = supplier;
        }
    }
}
