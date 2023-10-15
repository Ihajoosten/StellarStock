// Custom Imports
global using System.ComponentModel.DataAnnotations.Schema;
global using System.ComponentModel.DataAnnotations;

// Domain Layer Imports
global using StellarStock.Domain.Entities;
global using StellarStock.Domain.Repositories;
global using StellarStock.Domain.Repositories.Base;
global using StellarStock.Domain.Services.Interfaces;
global using StellarStock.Domain.Specifications.Interfaces;
global using StellarStock.Domain.ValueObjects;
global using StellarStock.Domain.Entities.Base;
global using StellarStock.Domain.Events.SupplierEvents;
global using StellarStock.Domain.Events.WarehouseEvents;
global using StellarStock.Domain.Events.ItemEvents;