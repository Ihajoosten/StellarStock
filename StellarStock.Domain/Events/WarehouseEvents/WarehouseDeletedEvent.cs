namespace StellarStock.Domain.Events.WarehouseEvents
{
    public class WarehouseDeletedEvent
    {
        public string LocationId { get; }

        public WarehouseDeletedEvent(string locationId)
        {
            LocationId = locationId;
        }
    }
}
