// Custom Imports
global using Serilog;
global using Microsoft.Extensions.Logging;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.EntityFrameworkCore.Infrastructure;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;

// Infrastructure Layer Imports
global using StellarStock.Infrastructure.Data.Interfaces;
global using StellarStock.Infrastructure.Repositories.Base;
global using StellarStock.Infrastructure.Data.Identity.Model;

// Domain Layer Imports
global using StellarStock.Domain.Entities;
global using StellarStock.Domain.Repositories;
global using StellarStock.Domain.Repositories.Base;
