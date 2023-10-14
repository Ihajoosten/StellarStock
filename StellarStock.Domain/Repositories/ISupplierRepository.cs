namespace StellarStock.Domain.Repositories
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<IEnumerable<Supplier>> GetActiveSuppliersAsync();
        Task<IEnumerable<Supplier>> GetSuppliersByCityAsync(string cityId);
        Task<IEnumerable<Supplier>> GetSuppliersByRegionAsync(string regionId);
        Task<IEnumerable<Supplier>> GetSuppliersWithValidityExpiringSoonAsync(DateTime expirationDate);

    }
}
