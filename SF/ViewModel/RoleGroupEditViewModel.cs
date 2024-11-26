using System.ComponentModel.DataAnnotations;

namespace SF.ViewModel
{
    public class RoleGroupEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<string> SelectedRoles { get; set; } = new List<string>();

        public List<RoleViewModel> AvailableRoles { get; set; } = new List<RoleViewModel>();
    }

}
