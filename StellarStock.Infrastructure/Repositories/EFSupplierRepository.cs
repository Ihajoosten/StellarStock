using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories;
using StellarStock.Infrastructure.Data.Interfaces;
using StellarStock.Infrastructure.Repositories.Base;

namespace StellarStock.Infrastructure.Repositories
{
    public class EFSupplierRepository : EFGenericRepository<Supplier>, ISupplierRepository
    {
        public EFSupplierRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<IEnumerable<Supplier>> GetActiveSuppliersAsync()
        {
            var suppliers = await _unitOfWork.GetRepository<Supplier>()!.GetAllAsync();
            return suppliers!.Where(supplier => supplier.IsActive).ToList();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByCityAsync(string cityId)
        {
            var suppliers = await _unitOfWork.GetRepository<Supplier>()!.GetAllAsync();
            return suppliers!.Where(supplier => supplier.Address.City == cityId).ToList();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByRegionAsync(string regionId)
        {
            var suppliers = await _unitOfWork.GetRepository<Supplier>()!.GetAllAsync();
            return suppliers!.Where(supplier => supplier.Address.Region == regionId).ToList();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersWithValidityExpiringSoonAsync(DateTime expirationDate)
        {
            var suppliers = await _unitOfWork.GetRepository<Supplier>()!.GetAllAsync();
            return suppliers!.Where(supplier => supplier.ValidityPeriod != null && supplier.ValidityPeriod.EndDate <= expirationDate).ToList();
        }
    }
}
