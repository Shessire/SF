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
                return RedirectToAction("Index", "BusinessPartner");
            }

            var addresses = await _context.Addresses
                .Where(a => a.BusinessPartnerId == partnerId)
                .Include(a => a.BusinessPartner)
                .ToListAsync();

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

            ViewData["BusinessPartners"] = new SelectList(_context.BusinessPartners, "Id", "Name", partnerId);
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
                return RedirectToAction(nameof(Index), new { partnerId = address.BusinessPartnerId });
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Error updating the address. Please ensure the business partner exists.");
                ViewData["BusinessPartners"] = new SelectList(await _context.BusinessPartners.ToListAsync(), "Id", "Name", address.BusinessPartnerId);
                return View(address);
            }
        }


        // GET: Delete confirmation view
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address); // Returns a view to confirm deletion
        }

        // POST: Handles the actual deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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




        public async Task<IActionResult> GetData(int partnerId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.BusinessPartnerId == partnerId)
                .Select(a => new
                {
                    a.Name,
                    a.AddressPri,
                    a.AddressOpt,
                    a.PostalCode,
                    a.Id
                })
                .ToListAsync();

            return Json(new { data = addresses });
        }


    }
}
