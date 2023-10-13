namespace StellarStock.Domain.Events.LocationEvents
{
    public class LocationDeletedEvent
    {
        public string LocationId { get; }

        public LocationDeletedEvent(string locationId)
        {
            LocationId = locationId;
        }
    }
}
