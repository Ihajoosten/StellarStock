using Serilog;

namespace StellarStock.Test.Config
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IApplicationDbContext, TestDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            // Application Service Dependencies
            services.AddScoped(typeof(ICommandDispatcher), typeof(CommandDispatcher));
            services.AddScoped(typeof(IQueryDispatcher), typeof(QueryDispatcher));
            services.AddScoped(typeof(IInventoryItemCommandHandler<>), typeof(InventoryItemCommandHandler<>));
            services.AddScoped(typeof(ISupplierCommandHandler<>), typeof(SupplierCommandHandler<>));
            services.AddScoped(typeof(IWarehouseCommandHandler<>), typeof(WarehouseCommandHandler<>));
            services.AddScoped(typeof(IInventoryItemQueryHandler<,>), typeof(InventoryItemQueryHandler<,>));
            services.AddScoped(typeof(ISupplierQueryHandler<,>), typeof(SupplierQueryHandler<,>));
            services.AddScoped(typeof(IWarehouseQueryHandler<,>), typeof(WarehouseQueryHandler<,>));

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

            services.AddLogging(builder =>
            {
                builder.AddConsole(); // Logs to the console
                builder.AddDebug();   // Logs to the debugger output
                builder.AddSerilog();
            });
        }
    }
}
