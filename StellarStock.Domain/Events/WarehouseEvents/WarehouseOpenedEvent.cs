namespace StellarStock.Domain.Events.WarehouseEvents
{
    public class WarehouseOpenedEvent
    {
        public Warehouse Location { get; }

        public WarehouseOpenedEvent(Warehouse location)
        {
            Location = location;
        }
    }
}
