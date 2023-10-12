using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events
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
