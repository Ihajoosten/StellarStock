using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events.SupplierEvents
{
    public class SupplierActivatedEvent
    {
        public Supplier Supplier { get; }

        public SupplierActivatedEvent(Supplier supplier)
        {
            Supplier = supplier;
        }
    }

}
