namespace StellarStock.Domain.Events.SupplierEvents
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
