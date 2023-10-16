// Customer package Imports
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Graph;
global using Microsoft.Identity.Web.Resource;
global using Microsoft.Identity.Web;

// Application Imports
global using StellarStock.Application.Commands.Concrete.InventoryItemCommands;
global using StellarStock.Application.Commands.Concrete.SupplierCommands;
global using StellarStock.Application.Commands.Concrete.WarehouseCommands;
global using StellarStock.Application.Commands.Handlers;
global using StellarStock.Application.Commands.Handlers.Interfaces;
global using StellarStock.Application.Dispatchers;
global using StellarStock.Application.Dispatchers.Interfaces;
global using StellarStock.Application.Dto;
global using StellarStock.Application.Queries.Concrete.InvetoryItemQueries;
global using StellarStock.Application.Queries.Concrete.SupplierQueries;
global using StellarStock.Application.Queries.Concrete.WarehouseQueries;
global using StellarStock.Application.Queries.Handlers;
global using StellarStock.Application.Queries.Handlers.Interfaces;

// Domain Imports
global using StellarStock.Domain.Repositories;
global using StellarStock.Domain.Repositories.Base;
global using StellarStock.Domain.Services;
global using StellarStock.Domain.Services.Interfaces;
global using StellarStock.Domain.Entities;

// Infrastructure Imports
global using StellarStock.Infrastructure.Data;
global using StellarStock.Infrastructure.Data.Identity;
global using StellarStock.Infrastructure.Data.Identity.Model;
global using StellarStock.Infrastructure.Data.Interfaces;
global using StellarStock.Infrastructure.Repositories;
global using StellarStock.Infrastructure.Repositories.Base;