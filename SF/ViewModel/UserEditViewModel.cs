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

        [DataType(DataType.Password)]
        [Display(Prompt = "Leave blank to keep current password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public string TelephoneNumber { get; set; }

        public string FaxNumber { get; set; }
    }
}
