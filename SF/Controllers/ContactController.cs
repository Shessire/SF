using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF.Data;
using SF.ViewModel;

namespace SF.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListByPartner(int partnerId)
        {
            // Fetch business partner name
            var partner = await _context.BusinessPartners.FindAsync(partnerId);
            if (partner == null)
            {
                return NotFound();
            }

            // Fetch addresses and their contacts
            var contacts = await _context.Addresses
                .Where(a => a.BusinessPartnerId == partnerId)
                .SelectMany(a => a.Contacts.Select(c => new ContactViewModel
                {
                    Address = $"{a.AddressPri}" + (string.IsNullOrWhiteSpace(a.AddressOpt) ? "" : $", {a.AddressOpt}"),
                    ContactName = c.Name,
                    Title = c.Title,
                    Tel = c.Tel,
                    Mobile = c.Mobile,
                    Email = c.Email
                }))
                .ToListAsync();


            // Pass data to the view
            var model = new ContactListViewModel
            {
                BusinessPartnerName = partner.Name,
                ContactsGroupedByAddress = contacts
                    .GroupBy(c => c.Address)
                    .ToDictionary(g => g.Key, g => g.ToList())
            };


            return View(model);
        }
    }
}
