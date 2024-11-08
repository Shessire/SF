using SF.Models;

namespace SF.ViewModel
{
    public class CompanyDetailsViewModel
    {
        public Company Company { get; set; }
        public Dictionary<string, string> UserRoles { get; set; }
    }
}
