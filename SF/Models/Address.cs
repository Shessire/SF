namespace SF.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
