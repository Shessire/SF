using System.ComponentModel.DataAnnotations;

namespace SF.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [Phone]
        public string Phone { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
