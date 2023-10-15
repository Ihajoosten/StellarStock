namespace StellarStock.Domain.Services.Interfaces
{
    public interface IWarehouseService
    {
        Task<bool> CanWarehouseBeUpdatedAsync(string warehouseId, string newStoreName, string newPhone, AddressVO newWarehouseAddress);
        Task<bool> CanWarehouseBeMovedAsync(string warehouseId, string newAddress, string newCity, string newRegion, string newCountry, string newPostalCode);
        Task<bool> CanWarehouseBeClosedAsync(string warehouseId, bool isOpen);
        Task<bool> CanWarehouseBeReopenedAsync(string warehouseId, bool isOpen);
        Task<bool> CanWarehouseBeDeletedAsync(string warehouseId);
    }
}
