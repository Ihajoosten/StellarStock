namespace StellarStock.Domain.Events.LocationEvents
{
    public class LocationReopenedEvent
    {
        public string LocationId { get; }

        public LocationReopenedEvent(string locationId)
        {
            LocationId = locationId;
        }
    }
}
