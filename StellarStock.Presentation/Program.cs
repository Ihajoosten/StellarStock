var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Identity Service
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi().AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph")).AddInMemoryTokenCaches();
services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityDbContext>().AddDefaultTokenProviders();

services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
});

// Infastructure Database Dependencies
services.AddTransient<IdentityDataSeeder>();
services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("StellarStockApiConnection")));
services.AddDbContext<IdentityDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("IdentityConnection")));

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

// Add Core Services
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddLogging(builder =>
{
    builder.AddConsole(); // Logs to the console
    builder.AddDebug();   // Logs to the debugger output
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed Identity Data
using (var scope = app.Services.CreateScope())
{
    var appServices = scope.ServiceProvider;
    try
    {
        var identityDataSeeder = appServices.GetRequiredService<IdentityDataSeeder>();
        await identityDataSeeder.SeedDataAsync();
    }
    catch (Exception ex)
    {
        var logger = appServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding identity data.");
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
