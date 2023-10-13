using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using StellarStock.Domain.Repositories.Base;
using StellarStock.Domain.Services;
using StellarStock.Domain.Services.Interfaces;
using StellarStock.Infrastructure.Data;
using StellarStock.Infrastructure.Data.Interfaces;
using StellarStock.Infrastructure.Repositories.Base;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Identity Service
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();

// Infastructure Database Dependencies
services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("StellarStockApiConnection")));

// Application Service Dependencies
/*services.AddScoped<IAppInventoryItemService, AppInventoryItemService>();
services.AddScoped<IAppWarehouseService, AppWarehouseService>();
services.AddScoped<IAppSupplierService, AppSupplierService>();*/

// Infrastructure Repository Dependencies
services.AddScoped(typeof(IGenericRepository<>), typeof(EFGenericRepository<>));
/*services.AddScoped<IInventoryItemRepository, EFInventoryItemRepository>();
services.AddScoped<IWarehouseRepository, EFWarehouseRepository>();
services.AddScoped<ISupplierRepository, EFSupplierRepository>();*/

// Domain Service Dependencies
services.AddScoped<IInventoryItemService, InventoryItemService>();
services.AddScoped<IWarehouseService, WarehouseService>();
services.AddScoped<ISupplierService, SupplierService>();

// Add Core Services
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
