using Microsoft.EntityFrameworkCore;
using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories;
using StellarStock.Infrastructure.Data.Interfaces;
using StellarStock.Infrastructure.Repositories.Base;

namespace StellarStock.Infrastructure.Repositories
{
    public class EFSupplierRepository : EFGenericRepository<Supplier>, ISupplierRepository
    {
        public EFSupplierRepository(IApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Supplier>> GetActiveSuppliersAsync()
        {
            return await _context.Suppliers.Where(supplier => supplier.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByCityAsync(string cityId)
        {
            return await _context.Suppliers.Where(supplier => supplier.Address.City == cityId).ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByRegionAsync(string regionId)
        {
            return await _context.Suppliers.Where(supplier => supplier.Address.Region == regionId).ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersWithValidityExpiringSoonAsync(DateTime expirationDate)
        {
            return await _context.Suppliers.Where(supplier => supplier.ValidityPeriod != null && supplier.ValidityPeriod.EndDate <= expirationDate).ToListAsync();
        }
    }
}
