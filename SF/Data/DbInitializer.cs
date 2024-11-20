using Microsoft.EntityFrameworkCore;
using SF.Models;

namespace SF.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        

        public DbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            await SeedCompaniesAsync();
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
    }
}
