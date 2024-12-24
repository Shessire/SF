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
                Address = $"{address.AddressPri}" + (string.IsNullOrWhiteSpace(address.AddressOpt) ? "" : $", {address.AddressOpt}"),
                BusinessPartnerName = address.BusinessPartner.Name,
                Contacts = contacts
            };

            return View(model);
        }

        // Create a Contact
        public IActionResult Create(int addressId)
        {
            var address = _context.Addresses
                .Include(a => a.BusinessPartner)
                .FirstOrDefault(a => a.Id == addressId);

            if (address == null)
            {
                return NotFound();
            }

            ViewData["Address"] = address;
            return View(new Contact { AddressId = addressId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { addressId = contact.AddressId });
            }

            var address = _context.Addresses
                .Include(a => a.BusinessPartner)
                .FirstOrDefault(a => a.Id == contact.AddressId);

            ViewData["Address"] = address;
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
    }
}
