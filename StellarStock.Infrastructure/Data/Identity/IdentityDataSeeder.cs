using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using StellarStock.Infrastructure.Data.Identity.Model;

namespace StellarStock.Infrastructure.Data.Identity
{
    public class IdentityDataSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataSeeder> _logger;

        public IdentityDataSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<IdentityDataSeeder> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task SeedDataAsync()
        {
            // Seed roles
            try
            {
                await SeedRolesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(message: ex.Message);
            }

            // Seed users
            try
            {
                await SeedUsersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(message: ex.Message);
            }
        }

        private async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                await _roleManager.CreateAsync(adminRole);
            }

            if (!await _roleManager.RoleExistsAsync("Guest"))
            {
                var guestRole = new IdentityRole("Guest");
                await _roleManager.CreateAsync(guestRole);
            }
        }

        private async Task SeedUsersAsync()
        {
            if (await _userManager.FindByEmailAsync("lhajoosten@stellarstock.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "lhajoosten@stellarstock.com",
                    Email = "lhajoosten@stellarstock.com",
                    PhoneNumber = "1234567890",
                };

                var result = await _userManager.CreateAsync(adminUser, "YourSecurePassword");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    _logger.LogWarning(message: "Could not create new User");
                }
            }
            if (await _userManager.FindByEmailAsync("guest@stellarstock.com") == null)
            {
                var guestUser = new ApplicationUser
                {
                    UserName = "guest@stellarstock.com",
                    Email = "guest@stellarstock.com",
                    PhoneNumber = "1234567890",
                };

                var result = await _userManager.CreateAsync(guestUser, "YourSecurePassword");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(guestUser, "Guest");
                }
                else
                {
                    _logger.LogWarning(message: "Could not create new User");
                }
            }
        }
    }
}
