using System.ComponentModel.DataAnnotations;

namespace SF.ViewModel
{
    public class UserCreateViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
    }
}
