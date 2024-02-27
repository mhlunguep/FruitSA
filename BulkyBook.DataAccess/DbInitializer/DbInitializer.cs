using FruitSA.Models;
using FruitSA.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FruitSA.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            // Migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                // Handle migration exception if necessary
            }

            // Create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();

                // If roles are not created, then we will create admin user as well
                if (_db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@fruitsa.com") == null)
                {
                    _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "AdminUser",
                        Email = "admin@fruitsa.com",
                        FirstName = "Admin",
                        LastName = "FruitSA"
                    }, "Admin123*").GetAwaiter().GetResult();

                    ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@fruitsa.com");

                    _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                }
            }

            // Insert dummy data for categories
            if (!_db.Categories.Any())
            {
                // Generate some dummy data
                var categories = new List<Category>
                {
                    new Category { CategoryName = "Front End", CategoryCode = "FRE401", IsActive = true, Username = "admin@fruitsa.com", CreatedDate = DateTime.Now, UpdatedAt = null },
                    new Category { CategoryName = "Back End", CategoryCode = "BKE602", IsActive = true, Username = "admin@fruitsa.com", CreatedDate = DateTime.Now, UpdatedAt = DateTime.Now },
                    new Category { CategoryName = "Database", CategoryCode = "DAB601", IsActive = true, Username = "admin@fruitsa.com", CreatedDate = DateTime.Now, UpdatedAt = null },
                   
                };

                _db.Categories.AddRange(categories);
                _db.SaveChanges();
            }
        }
    }
}
