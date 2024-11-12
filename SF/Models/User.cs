using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace SF.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
    }
}
