using StellarStock.Domain.ValueObjects;

namespace StellarStock.Domain.Services.Interfaces
{
    public interface ILocationService
    {
        Task<bool> CanLocationBeUpdatedAsync(string locationId, string newStoreName, string newPhone, AddressVO newLocationAddress);
        Task<bool> CanLocationBeMovedAsync(string locationId, string newAddress, string newCity, string newRegion, string newCountry, string newPostalCode);
        Task<bool> CanLocationBeClosedAsync(string locationId, bool isOpen);
        Task<bool> CanLocationBeReopenedAsync(string locationId, bool isOpen);
        Task<bool> CanLocationBeDeletedAsync(string locationId);
    }
}
