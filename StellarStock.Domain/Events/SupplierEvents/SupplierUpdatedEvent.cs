using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events.SupplierEvents
{
    public class SupplierUpdatedEvent
    {
        public Supplier Supplier { get; }

        public SupplierUpdatedEvent(Supplier supplier)
        {
            Supplier = supplier;
        }
    }
}
