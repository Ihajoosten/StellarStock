using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace StellarStock.Test.Config
{
    public class TestDbContext : DbContext, IApplicationDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
        {
        }

        // Add DbSet properties for each entity you want to test
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
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

            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.OwnsOne(e => e.ProductCode, productCode =>
                {
                    productCode.Property(a => a.Code).HasColumnName("Code");
                });

                entity.OwnsOne(e => e.Quantity, quantity =>
                {
                    quantity.Property(a => a.Value).HasColumnName("Value");
                });

                entity.OwnsOne(e => e.Money, money =>
                {
                    money.Property(a => a.Amount).HasColumnName("Amount");
                    money.Property(a => a.Currency).HasColumnName("Currency");
                });

                entity.OwnsOne(e => e.ValidityPeriod, validityPeriod =>
                {
                    validityPeriod.Property(a => a.StartDate).HasColumnName("StartDate");
                    validityPeriod.Property(a => a.EndDate).HasColumnName("EndDate");
                });
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.OwnsOne(e => e.Address, address =>
                {
                    address.Property(a => a.Street).HasColumnName("Street");
                    address.Property(a => a.City).HasColumnName("City");
                    address.Property(a => a.City).HasColumnName("Region");
                    address.Property(a => a.City).HasColumnName("PostalCode");
                    address.Property(a => a.City).HasColumnName("Country");
                });
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.OwnsOne(e => e.Address, address =>
                {
                    address.Property(a => a.Street).HasColumnName("Street");
                    address.Property(a => a.City).HasColumnName("City");
                    address.Property(a => a.City).HasColumnName("Region");
                    address.Property(a => a.City).HasColumnName("PostalCode");
                    address.Property(a => a.City).HasColumnName("Country");
                });

                entity.OwnsOne(e => e.ValidityPeriod, validityPeriod =>
                {
                    validityPeriod.Property(a => a.StartDate).HasColumnName("StartDate");
                    validityPeriod.Property(a => a.EndDate).HasColumnName("EndDate");
                });
            });
        }

    }
}
