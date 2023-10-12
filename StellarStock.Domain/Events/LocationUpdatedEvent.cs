using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events
{
    public class LocationUpdatedEvent
    {
        public Location Location { get; }

        public LocationUpdatedEvent(Location location)
        {
            Location = location;
        }
    }
}
