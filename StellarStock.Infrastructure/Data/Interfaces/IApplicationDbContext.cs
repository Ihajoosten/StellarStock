using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StellarStock.Domain.Entities;

namespace StellarStock.Infrastructure.Data.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<InventoryItem> InventoryItems { get; set; }
        DbSet<Warehouse> Warehouses { get; set; }
        DbSet<Supplier> Suppliers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
