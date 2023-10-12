using StellarStock.Domain.Entities;
using StellarStock.Domain.ValueObjects;

namespace StellarStock.Domain.Aggregates
{
    public class LocationAggregate
    {
        public Location? Location { get; private set; }

        public LocationAggregate(Location location)
        {
            ValidateAndSetProperties(location);
        }

        private void ValidateAndSetProperties(Location location)
        {
            // Basic validation
            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            // Set properties
            Location = location;
        }

        public void UpdateLocationInformation(string newStoreName, string newPhone, AddressVO newLocationAddress)
        {
            // Check if the location is valid or throw an exception if needed
            if (Location == null)
            {
                throw new InvalidOperationException("Location is not valid.");
            }

            // Update location information
            Location.StoreName = newStoreName;
            Location.Phone = newPhone;
            Location.LocationAddress = newLocationAddress;
        }
    }
}
