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
                _logger.LogCritical(message: $"Message :: {ex.Message}");
                _logger.LogCritical(message: $"Stacktrade :: {ex.StackTrace}");
            }

            // Seed users
            try
            {
                await SeedUsersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(message: $"Message :: {ex.Message}");
                _logger.LogCritical(message: $"Stacktrade :: {ex.StackTrace}");
            }
        }

        private async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                try
                {
                    var adminRole = new IdentityRole("Admin");
                    await _roleManager.CreateAsync(adminRole);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(message: $"Message :: {ex.Message}");
                    _logger.LogCritical(message: $"Stacktrade :: {ex.StackTrace}");
                }
            }

            if (!await _roleManager.RoleExistsAsync("Guest"))
            {
                try
                {
                    var guestRole = new IdentityRole("Guest");
                    await _roleManager.CreateAsync(guestRole);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(message: $"Message :: {ex.Message}");
                    _logger.LogCritical(message: $"Stacktrade :: {ex.StackTrace}");
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            var adminuser = await _userManager.FindByNameAsync("lhajoosten@stellarstock.com");
            var guestUser = await _userManager.FindByNameAsync("guest@stellarstock.com");

            if (adminuser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "lhajoosten@stellarstock.com",
                    Email = "lhajoosten@stellarstock.com",
                };
                try
                {
                    var result = await _userManager.CreateAsync(admin, "AdminP@ssw0rd");

                    if (result.Succeeded)
                    {
                        try
                        {
                            await _userManager.AddToRoleAsync(admin, "Admin");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogCritical(message: $"Message :: {ex.Message}");
                            _logger.LogCritical(message: $"Stacktrade :: {ex.StackTrace}");
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogWarning(message: $"Error: {error.Description}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(message: $"Message :: {ex.Message}");
                    _logger.LogCritical(message: $"Stacktrade :: {ex.StackTrace}");
                }
            }
            if (guestUser == null)
            {
                var guest = new ApplicationUser
                {
                    UserName = "guest@stellarstock.com",
                    Email = "guest@stellarstock.com",
                };

                try
                {
                    var result = await _userManager.CreateAsync(guest, "GuestP@ssw0rd");

                    if (result.Succeeded)
                    {
                        try
                        {
                            await _userManager.AddToRoleAsync(guest, "Guest");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogCritical(message: $"Message :: {ex.Message}");
                            _logger.LogCritical(message: $"Stacktrade :: {ex.StackTrace}");
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogWarning(message: $"Error: {error.Description}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(message: $"Message :: {ex.Message}");
                    _logger.LogCritical(message: $"Stacktrade :: {ex.StackTrace}");
                }
            }
        }
    }
}
