using SF.Models;

namespace SF.ViewModel
{
    public class ContactListViewModel
    {
        public string BusinessPartnerName { get; set; }
        public string Address { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
