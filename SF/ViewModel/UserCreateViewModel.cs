using System.ComponentModel.DataAnnotations;

namespace SF.ViewModel
{
    public class UserCreateViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
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
