// NuGet Package Imports
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Moq;
// Application Layer Imports
global using StellarStock.Application.Commands.Concrete.InventoryItemCommands;
global using StellarStock.Application.Commands.Concrete.SupplierCommands;
global using StellarStock.Application.Commands.Handlers;
global using StellarStock.Application.Commands.Handlers.Interfaces;
global using StellarStock.Application.Dispatchers;
global using StellarStock.Application.Dispatchers.Interfaces;
global using StellarStock.Application.Queries.Handlers;
global using StellarStock.Application.Queries.Handlers.Interfaces;
// Domain Layer Imports
global using StellarStock.Domain.Aggregates;
global using StellarStock.Domain.Entities;
global using StellarStock.Domain.Events.ItemEvents;
global using StellarStock.Domain.Events.SupplierEvents;
global using StellarStock.Domain.Events.WarehouseEvents;
global using StellarStock.Domain.Repositories;
global using StellarStock.Domain.Repositories.Base;
global using StellarStock.Domain.Services;
global using StellarStock.Domain.Services.Interfaces;
global using StellarStock.Domain.ValueObjects;
// Infrastructure Layer Imports
global using StellarStock.Infrastructure.Data;
global using StellarStock.Infrastructure.Data.Interfaces;
global using StellarStock.Infrastructure.Repositories;
global using StellarStock.Infrastructure.Repositories.Base;
// Presentation Layer Imports


// Test Layer imports
global using StellarStock.Test.Config;
global using Xunit;
