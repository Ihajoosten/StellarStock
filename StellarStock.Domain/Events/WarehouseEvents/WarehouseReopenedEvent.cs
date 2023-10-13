namespace StellarStock.Domain.Events.WarehouseEvents
{
    public class WarehouseReopenedEvent
    {
        public string LocationId { get; }

        public WarehouseReopenedEvent(string locationId)
        {
            LocationId = locationId;
        }
    }
}
