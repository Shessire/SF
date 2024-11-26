using SF.Models;

namespace SF.ViewModel
{
    public class CompanyDetailsViewModel
    {
        public Company Company { get; set; }
        public Dictionary<string, List<string>> UserRoles { get; set; }
    }
}
