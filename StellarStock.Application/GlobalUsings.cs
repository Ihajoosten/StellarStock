// Custom Imports
global using AutoMapper;
global using Microsoft.Extensions.Logging;

// Application Layer Imports
global using StellarStock.Application.Interfaces;
global using StellarStock.Application.Interfaces.Handler;
global using StellarStock.Application.Interfaces.Handler.Base;
global using StellarStock.Application.Queries.InventoryItemQueries;
global using StellarStock.Application.Queries.SupplierQueries;
global using StellarStock.Application.Queries.WarehouseQueries;
global using StellarStock.Application.Commands.InventoryItemCommands;
global using StellarStock.Application.Commands.SupplierCommands;
global using StellarStock.Application.Commands.WarehouseCommands;
global using StellarStock.Application.ExceptionHandling;

// Domain Layer Imports
global using StellarStock.Domain.Entities;
global using StellarStock.Domain.Repositories;
global using StellarStock.Domain.Repositories.Base;
global using StellarStock.Domain.ValueObjects;
