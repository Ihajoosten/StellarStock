namespace StellarStock.Domain.Events.WarehouseEvents
{
    public class WarehouseClosedEvent
    {
        public string LocationId { get; }

        public WarehouseClosedEvent(string locationId)
        {
            LocationId = locationId;
        }
    }
}
