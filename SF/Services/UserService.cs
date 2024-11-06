using Microsoft.AspNetCore.Identity;
using SF.Data;
using SF.Models;

namespace SF.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> AssignUserToCompanyAsync(string userId, int companyId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var company = await _context.Companies.FindAsync(companyId);
            if (company == null) return false;

            user.CompanyId = companyId;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
