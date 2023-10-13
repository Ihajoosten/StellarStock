using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StellarStock.Domain.Entities;
using StellarStock.Infrastructure.Data.Interfaces;

namespace StellarStock.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Implement any additional logic before saving changes if needed
            return base.SaveChangesAsync(cancellationToken);
        }

        public override DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public override EntityEntry<T> Entry<T>(T entity) where T : class
        {
            return base.Entry(entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InventoryItem>()
                .HasOne(item => item.Supplier)
                .WithMany(supplier => supplier.SuppliedItems)
                .HasForeignKey(item => item.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryItem>()
                .HasOne(item => item.Warehouse)
                .WithMany(supplier => supplier.StockedItems)
                .HasForeignKey(item => item.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
