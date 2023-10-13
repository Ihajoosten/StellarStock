using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events.SupplierEvents
{
    public class SupplierDeletedEvent
    {
        public Supplier Supplier { get; }

        public SupplierDeletedEvent(Supplier supplier)
        {
            Supplier = supplier;
        }
    }
}
