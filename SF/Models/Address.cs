namespace SF.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressPri { get; set; } // Primary Address Line
        public string? AddressOpt { get; set; } // Optional Address Line
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string? TaxBranchCode { get; set; } // Optional Tax Branch Code

        public int BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
