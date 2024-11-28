namespace SF.ViewModel
{
    public class ContactListViewModel
    {
        public string BusinessPartnerName { get; set; }
        public Dictionary<string, List<ContactViewModel>> ContactsGroupedByAddress { get; set; }
    }
}
