using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SF.ViewModel
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Company")]
        public int? CompanyId { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public List<string> SelectedRoles { get; set; } = new();

        public List<RoleViewModel> Roles { get; set; } = new();

        public string TelephoneNumber { get; set; }

        public string FaxNumber { get; set; }
    }

    public class RoleViewModel
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
