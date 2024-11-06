using SF.Models;

namespace SF.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<bool> AssignUserToCompanyAsync(string userId, int  companyId);
    }
}
