// NuGet Package Imports
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;
global using Moq;
// Domain Layer Imports
global using StellarStock.Domain.Aggregates;
global using StellarStock.Domain.Entities;
global using StellarStock.Domain.Events.ItemEvents;
global using StellarStock.Domain.Events.SupplierEvents;
global using StellarStock.Domain.Events.WarehouseEvents;
global using StellarStock.Domain.Repositories.Base;
global using StellarStock.Domain.ValueObjects;
// Application Layer Imports


// Infrastructure Layer Imports
global using StellarStock.Infrastructure.Data;
global using StellarStock.Infrastructure.Data.Interfaces;
global using StellarStock.Infrastructure.Repositories.Base;
// Presentation Layer Imports


// Test Layer imports
global using StellarStock.Test.Config;
global using Xunit;
