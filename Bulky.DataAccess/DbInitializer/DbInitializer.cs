using BulkyBook.DataAcess.Data;
using BulkyBook.Models.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            ILogger<DbInitializer> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _logger = logger;
        }

        public void Initialize()
        {
            // Apply any pending migrations
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                    _logger.LogInformation("Database migrations applied successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Migration error: {ex.Message}");
            }

            // Create roles if they don't exist
            var roles = new List<string>
            {
                SD.Role_Customer,
                SD.Role_Employee,
                SD.Role_Admin,
                SD.Role_Company
            };

            foreach (var role in roles)
            {
                if (!_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    var result = _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"Role '{role}' created successfully.");
                    }
                    else
                    {
                        _logger.LogError($"Error creating role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }

            // Create admin user if not exists
            var adminEmail = "admin@gmail.com";
            if (_userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult() == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Roman Shahrear",
                    PhoneNumber = "01786175147",
                    StreetAddress = "A/2",
                    State = "Mymenshingh",
                    PostalCode = "2000",
                    City = "Jamalpur"
                };

                var createAdminResult = _userManager.CreateAsync(adminUser, "Asp.net123456!").GetAwaiter().GetResult();
                if (createAdminResult.Succeeded)
                {
                    _userManager.AddToRoleAsync(adminUser, SD.Role_Admin).GetAwaiter().GetResult();
                    _logger.LogInformation($"Admin user '{adminEmail}' created and assigned to role '{SD.Role_Admin}'.");
                }
                else
                {
                    _logger.LogError($"User creation error for '{adminEmail}': {string.Join(", ", createAdminResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
