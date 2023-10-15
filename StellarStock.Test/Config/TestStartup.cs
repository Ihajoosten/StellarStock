using Microsoft.Extensions.DependencyInjection;
using StellarStock.Domain.Repositories;
using StellarStock.Domain.Services;
using StellarStock.Domain.Services.Interfaces;
using StellarStock.Infrastructure.Repositories;

namespace StellarStock.Test.Config
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IApplicationDbContext, TestDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            // Infrastructure Repository Dependencies
            services.AddScoped(typeof(IGenericRepository<>), typeof(EFGenericRepository<>));
            services.AddScoped<IInventoryItemRepository, EFInventoryItemRepository>();
            services.AddScoped<IWarehouseRepository, EFWarehouseRepository>();
            services.AddScoped<ISupplierRepository, EFSupplierRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Domain Service Dependencies
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<ISupplierService, SupplierService>();
        }
    }
}
