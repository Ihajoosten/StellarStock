// Custom Imports
global using AutoMapper;
global using Microsoft.Extensions.Logging;

// Application Layer Imports
global using StellarStock.Application.Commands.Concrete.InventoryItemCommands;
global using StellarStock.Application.Commands.Concrete.SupplierCommands;
global using StellarStock.Application.Commands.Concrete.WarehouseCommands;
global using StellarStock.Application.Commands.Handlers.Interfaces;
global using StellarStock.Application.Commands.Interfaces;
global using StellarStock.Application.Dispatchers.Interfaces;
global using StellarStock.Application.Queries.Concrete.InvetoryItemQueries;
global using StellarStock.Application.Queries.Concrete.SupplierQueries;
global using StellarStock.Application.Queries.Concrete.WarehouseQueries;
global using StellarStock.Application.Queries.Handlers.Interfaces;
global using StellarStock.Application.Queries.Interfaces;

// Domain Layer Imports
global using StellarStock.Domain.Aggregates;
global using StellarStock.Domain.Entities;
global using StellarStock.Domain.Repositories;
global using StellarStock.Domain.Repositories.Base;
global using StellarStock.Domain.ValueObjects;

