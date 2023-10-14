namespace StellarStock.Infrastructure.Repositories
{
    public class EFSupplierRepository : EFGenericRepository<Supplier>, ISupplierRepository
    {
        public EFSupplierRepository(IUnitOfWork unitOfWork, ILogger<EFSupplierRepository> logger) : base(unitOfWork, logger) { }

        public async Task<IEnumerable<Supplier>> GetActiveSuppliersAsync()
        {
            try
            {
                var suppliers = await _unitOfWork.GetRepository<Supplier>()!.GetAllAsync();
                var activeSuppliers = suppliers!.Where(supplier => supplier.IsActive).ToList();
                _logger.LogInformation($"Retrieved {activeSuppliers.Count} active suppliers");
                return activeSuppliers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving active suppliers");
                throw;
            }
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByCityAsync(string city)
        {
            try
            {
                var suppliers = await _unitOfWork.GetRepository<Supplier>()!.GetAllAsync();
                var suppliersByCity = suppliers!.Where(supplier => supplier.Address.City == city).ToList();
                _logger.LogInformation($"Retrieved {suppliersByCity.Count} suppliers by city '{city}'");
                return suppliersByCity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving suppliers by city '{city}'");
                throw;
            }
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByRegionAsync(string region)
        {
            try
            {
                var suppliers = await _unitOfWork.GetRepository<Supplier>()!.GetAllAsync();
                var suppliersByRegion = suppliers!.Where(supplier => supplier.Address.Region == region).ToList();
                _logger.LogInformation($"Retrieved {suppliersByRegion.Count} suppliers by region '{region}'");
                return suppliersByRegion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving suppliers by region '{region}'");
                throw;
            }
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersWithValidityExpiringSoonAsync(DateTime expirationDate)
        {
            try
            {
                var suppliers = await _unitOfWork.GetRepository<Supplier>()!.GetAllAsync();
                var expiringSoonSuppliers = suppliers!.Where(supplier => supplier.ValidityPeriod != null && supplier.ValidityPeriod.EndDate <= expirationDate).ToList();
                _logger.LogInformation($"Retrieved {expiringSoonSuppliers.Count} suppliers with validity expiring soon");
                return expiringSoonSuppliers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving suppliers with validity expiring soon");
                throw;
            }
        }
    }
}
