using StellarStock.Domain.ValueObjects;

namespace StellarStock.Domain.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<bool> CanSupplierBeUpdatedAsync(string supplierId, string newName, string newPhone, string newEmail, AddressVO newAddress);
        Task<bool> CanSupplierBeDeletedAsync(string supplierId);
        Task<bool> CanSupplierBeActivatedAsync(string supplierId);
        Task<bool> CanSupplierBeDeactivatedAsync(string supplierId);
    }
}
