using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF.Data;
using SF.Models;
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

        public async Task<IActionResult> Index(int addressId)
        {
            ViewData["AddressId"] = addressId;

            var address = await _context.Addresses
                .Include(a => a.BusinessPartner)
                .FirstOrDefaultAsync(a => a.Id == addressId);

            if (address == null)
            {
                return NotFound();
            }

            var contacts = await _context.Contacts
                .Where(c => c.AddressId == addressId)
                .ToListAsync();

            var model = new ContactListViewModel
            {
                AddressId = addressId,
                AddressName = address.Name,
                BusinessPartnerName = address.BusinessPartner.Name,
                BusinessPartnerId = address.BusinessPartnerId,
                Contacts = contacts
            };

            return View(model);
        }

        public IActionResult Create(int addressId)
        {
            // Ensure the Address exists before proceeding
            var address = _context.Addresses
                .Include(a => a.BusinessPartner)
                .FirstOrDefault(a => a.Id == addressId);

            if (address == null)
            {
                return NotFound("Address not found.");
            }

            ViewData["Address"] = $"{address.AddressPri}" +
                                  (!string.IsNullOrWhiteSpace(address.AddressOpt) ? $", {address.AddressOpt}" : "");
            ViewData["BusinessPartnerName"] = address.BusinessPartner.Name;

            // Pass AddressId to the view
            return View(new Contact { AddressId = addressId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact)
        {
            // Validate Address existence
            var address = _context.Addresses.FirstOrDefault(a => a.Id == contact.AddressId);
            if (address == null)
            {
                ModelState.AddModelError("", "Associated Address is invalid or does not exist.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { addressId = contact.AddressId });
            }

            // Provide context back to the View in case of errors
            if (address != null)
            {
                ViewData["Address"] = $"{address.AddressPri}" +
                                      (!string.IsNullOrWhiteSpace(address.AddressOpt) ? $", {address.AddressOpt}" : "");
                ViewData["BusinessPartnerName"] = address.BusinessPartner?.Name;
            }

            return View(contact);
        }


        // Edit a Contact
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.BusinessPartner)
                .FirstOrDefaultAsync(a => a.Id == contact.AddressId);

            ViewData["Address"] = address;
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index), new { addressId = contact.AddressId });
            }

            var address = await _context.Addresses
                .Include(a => a.BusinessPartner)
                .FirstOrDefaultAsync(a => a.Id == contact.AddressId);

            ViewData["Address"] = address;
            return View(contact);
        }

        // Delete a Contact
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts
                .Include(c => c.Address)
                .ThenInclude(a => a.BusinessPartner)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new { addressId = contact?.AddressId });
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AllContactsForPartner(int partnerId)
        {
            var contacts = await _context.Contacts
                .Include(c => c.Address) // Include related Address
                .Where(c => c.Address.BusinessPartnerId == partnerId) // Filter by BusinessPartnerId
                .ToListAsync();

            var partner = await _context.BusinessPartners
                .FirstOrDefaultAsync(bp => bp.Id == partnerId);

            if (partner == null)
            {
                return NotFound();
            }

            ViewData["PartnerId"] = partnerId;
            ViewData["PartnerName"] = partner.Name;

            return View(contacts);
        }

        public async Task<IActionResult> GetData(int addressId)
        {
            var contacts = await _context.Contacts
                .Where(c => c.AddressId == addressId)
                .Select(c => new
                {
                    c.Title,
                    c.Name,
                    c.Tel,
                    c.Mobile,
                    c.Email,
                    c.Id
                }).ToListAsync();

            return Json(new { data = contacts });
        }

        public async Task<IActionResult> GetContactsForPartner(int partnerId)
        {
            var contacts = await _context.Contacts
                .Include(c => c.Address) // Include related Address
                .Where(c => c.Address.BusinessPartnerId == partnerId) // Filter by partnerId
                .Select(c => new
                {
                    c.Title,
                    c.Name,
                    c.Tel,
                    c.Mobile,
                    c.Email,
                    AddressName = c.Address.Name, // Include the Address name
                    c.Id
                }).ToListAsync();

            return Json(new { data = contacts });
        }


    }
}
