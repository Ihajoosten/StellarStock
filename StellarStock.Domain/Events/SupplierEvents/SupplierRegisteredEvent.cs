using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events.SupplierEvents
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
