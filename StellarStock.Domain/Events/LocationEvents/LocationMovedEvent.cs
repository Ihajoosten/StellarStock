using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events.LocationEvents
{
    public class LocationMovedEvent
    {
        public Location Location { get; }

        public LocationMovedEvent(Location location)
        {
            Location = location;
        }
    }
}
