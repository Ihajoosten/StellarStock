using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events.WarehouseEvents
{
    public class WarehouseMovedEvent
    {
        public Warehouse Location { get; }

        public WarehouseMovedEvent(Warehouse location)
        {
            Location = location;
        }
    }
}
