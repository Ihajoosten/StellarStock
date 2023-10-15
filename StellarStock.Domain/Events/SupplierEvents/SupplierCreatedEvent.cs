namespace StellarStock.Domain.Events.SupplierEvents
{
    public class SupplierCreatedEvent
    {
        public Supplier Supplier { get; }

        public SupplierCreatedEvent(Supplier supplier)
        {
            Supplier = supplier;
        }
    }
}
