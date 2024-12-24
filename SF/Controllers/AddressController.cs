using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using SF.Data;
using SF.Models;
using Microsoft.EntityFrameworkCore;

namespace SF.Controllers
{
    public class AddressController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? partnerId)
        {
            if (partnerId == null)
            {
                // Optionally redirect to an error page or BusinessPartners list if partnerId is missing
                return RedirectToAction("Index", "BusinessPartner");
            }

            // Fetch addresses specific to the BusinessPartner
            var addresses = await _context.Addresses
                .Where(a => a.BusinessPartnerId == partnerId)
                .Include(a => a.BusinessPartner)
                .ToListAsync();

            // Pass BusinessPartnerId to the ViewBag for navigation
            ViewBag.PartnerId = partnerId;

            return View(addresses);
        }

        public IActionResult Create(int partnerId)
        {
            var businessPartner = _context.BusinessPartners.FirstOrDefault(bp => bp.Id == partnerId);
            if (businessPartner == null)
            {
                return RedirectToAction("Index", "BusinessPartner");
            }

            ViewData["BusinessPartner"] = businessPartner;
            return View(new Address { BusinessPartnerId = partnerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { partnerId = address.BusinessPartnerId });
            }
            ViewData["BusinessPartners"] = new SelectList(_context.BusinessPartners, "Id", "Name", address.BusinessPartnerId);
            return View(address);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            ViewData["BusinessPartners"] = new SelectList(await _context.BusinessPartners.ToListAsync(), "Id", "Name", address.BusinessPartnerId);
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Address address)
        {
            if (!ModelState.IsValid)
            {
                ViewData["BusinessPartners"] = new SelectList(await _context.BusinessPartners.ToListAsync(), "Id", "Name", address.BusinessPartnerId);
                return View(address);
            }

            try
            {
                _context.Update(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Error updating the address. Please ensure the business partner exists.");
                ViewData["BusinessPartners"] = new SelectList(await _context.BusinessPartners.ToListAsync(), "Id", "Name", address.BusinessPartnerId);
                return View(address);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            int partnerId = address.BusinessPartnerId;

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { partnerId });
        }

    }
}
