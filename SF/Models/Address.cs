using System.ComponentModel.DataAnnotations;

namespace SF.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Address")]
        public string AddressPri { get; set; } // Primary Address Line

        [Display(Name = "Address 2 (optional)")]
        public string? AddressOpt { get; set; } // Optional Address Line

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        public string Country { get; set; }

        [Display(Name = "Tax branch code")]
        public string? TaxBranchCode { get; set; } // Optional Tax Branch Code

        [Display(Name = "Business Partner")]
        public int BusinessPartnerId { get; set; }
        public BusinessPartner? BusinessPartner { get; set; }
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
