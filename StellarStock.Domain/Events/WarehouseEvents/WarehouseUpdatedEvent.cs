namespace StellarStock.Domain.Events.WarehouseEvents
{
    public class WarehouseUpdatedEvent
    {
        public Warehouse Location { get; }

        public WarehouseUpdatedEvent(Warehouse location)
        {
            Location = location;
        }
    }
}
