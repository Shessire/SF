using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SF.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
                
        public int AddressId { get; set; }
        [ValidateNever]
        public Address Address { get; set; }
    }
}
