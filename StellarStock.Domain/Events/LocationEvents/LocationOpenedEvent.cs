using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events.LocationEvents
{
    public class LocationOpenedEvent
    {
        public Location Location { get; }

        public LocationOpenedEvent(Location location)
        {
            Location = location;
        }
    }
}
