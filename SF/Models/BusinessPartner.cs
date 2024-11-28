using System.Net;

namespace SF.Models
{
    public class BusinessPartner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; } // "Customer" or "Supplier"
        public string EntityType { get; set; } // "Corporate" or "Individual"
        public ICollection<Address>? Addresses { get; set; }
    }
}
