// Customer package Imports
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Identity.Web;

// Applicationg Imports
global using StellarStock.Application.Handlers.CommandHandlers;
global using StellarStock.Application.Handlers.QueryHandlers;
global using StellarStock.Application.Interfaces.Handler;
global using StellarStock.Application.Interfaces.Handler.Base;

// Domain Imports
global using StellarStock.Domain.Repositories;
global using StellarStock.Domain.Repositories.Base;
global using StellarStock.Domain.Services;
global using StellarStock.Domain.Services.Interfaces;

// Infrastructure Imports
global using StellarStock.Infrastructure.Data;
global using StellarStock.Infrastructure.Data.Identity;
global using StellarStock.Infrastructure.Data.Identity.Model;
global using StellarStock.Infrastructure.Data.Interfaces;
global using StellarStock.Infrastructure.Repositories;
global using StellarStock.Infrastructure.Repositories.Base;