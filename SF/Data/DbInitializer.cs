using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SF.Models;

namespace SF.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            await SeedCompaniesAsync();
            await SeedSuperAdminAsync();
        }

        private async Task SeedCompaniesAsync()
        {
            var companies = new List<Company>
        {
            new Company
            {
                Name = "SF",
                Address = "123 Market Street",
                Phone = "+1 (415) 555-0123"
            },
            new Company
            {
                Name = "Mango",
                Address = "456 Innovation Drive",
                Phone = "+1 (512) 555-0456",
            }
        };

            foreach (var company in companies)
            {
                var existingCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Name == company.Name);
                if (existingCompany == null)
                {
                    await _context.Companies.AddAsync(company);
                }
            }

            await _context.SaveChangesAsync();
        }
        private async Task SeedSuperAdminAsync()
        {
            // Ensure SuperAdmin role exists
            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            var superAdminEmail = "superadmin@gmail.com";
            var superAdminPassword = "Test123;";

            // Check if SuperAdmin user exists
            var existingUser = await _userManager.FindByEmailAsync(superAdminEmail);

            if (existingUser == null)
            {
                var superAdmin = new ApplicationUser
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    FirstName = "Super",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    FaxNumber = "N/A"
                };

                var result = await _userManager.CreateAsync(superAdmin, superAdminPassword); // Strong default password

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating SuperAdmin: {error.Description}");
                    }
                }
            }
        }
    }
}
