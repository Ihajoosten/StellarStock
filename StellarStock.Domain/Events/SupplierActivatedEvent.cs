using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events
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
