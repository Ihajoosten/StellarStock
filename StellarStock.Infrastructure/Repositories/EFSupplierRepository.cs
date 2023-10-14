using Microsoft.Extensions.Logging;
using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories;
using StellarStock.Infrastructure.Data.Interfaces;
using StellarStock.Infrastructure.Repositories.Base;

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

        public async Task<IEnumerable<Supplier>> GetSuppliersByCityAsync(string cityId)
        {
            try
            {
                var suppliers = await _unitOfWork.GetRepository<Supplier>()!.GetAllAsync();
                var suppliersByCity = suppliers!.Where(supplier => supplier.Address.City == cityId).ToList();
                _logger.LogInformation($"Retrieved {suppliersByCity.Count} suppliers by city '{cityId}'");
                return suppliersByCity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving suppliers by city '{cityId}'");
                throw;
            }
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByRegionAsync(string regionId)
        {
            try
            {
                var suppliers = await _unitOfWork.GetRepository<Supplier>()!.GetAllAsync();
                var suppliersByRegion = suppliers!.Where(supplier => supplier.Address.Region == regionId).ToList();
                _logger.LogInformation($"Retrieved {suppliersByRegion.Count} suppliers by region '{regionId}'");
                return suppliersByRegion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving suppliers by region '{regionId}'");
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
