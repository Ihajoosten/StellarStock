// NuGet Package Imports
global using Xunit;
global using Moq;
global using Microsoft.Extensions.Logging;
global using Microsoft.EntityFrameworkCore;

// Domain Layer Imports
global using StellarStock.Domain.Aggregates;
global using StellarStock.Domain.Entities;
global using StellarStock.Domain.Events.ItemEvents;
global using StellarStock.Domain.Events.SupplierEvents;
global using StellarStock.Domain.Events.WarehouseEvents;
global using StellarStock.Domain.ValueObjects;
global using StellarStock.Domain.Repositories.Base;

// Application Layer Imports
global using StellarStock.Application.Commands.InventoryItemCommands;
global using StellarStock.Application.Handlers.CommandHandlers;

// Infrastructure Layer Imports
global using StellarStock.Infrastructure.Data;
global using StellarStock.Infrastructure.Data.Interfaces;
global using StellarStock.Infrastructure.Repositories.Base;


// Presentation Layer Imports


// Test Layer imports
global using StellarStock.Test.Config;
