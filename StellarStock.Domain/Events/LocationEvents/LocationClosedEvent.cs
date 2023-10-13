namespace StellarStock.Domain.Events.LocationEvents
{
    public class LocationClosedEvent
    {
        public string LocationId { get; }

        public LocationClosedEvent(string locationId)
        {
            LocationId = locationId;
        }
    }
}
