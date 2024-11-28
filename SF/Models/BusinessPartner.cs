using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SF.Models
{
    public class BusinessPartner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        
        public string EntityType { get; set; }
        public ICollection<Address>? Addresses { get; set; } = new List<Address>();
    }
}
