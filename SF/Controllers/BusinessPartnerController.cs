using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF.Data;
using SF.Models;

namespace SF.Controllers
{
    //[Authorize]
    public class BusinessPartnerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinessPartnerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var partners = await _context.BusinessPartners.Include(bp => bp.Addresses).ToListAsync();
            return View(partners);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessPartner partner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partner);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var partner = await _context.BusinessPartners.Include(bp => bp.Addresses).FirstOrDefaultAsync(bp => bp.Id == id);
            if (partner == null) return NotFound();
            return View(partner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BusinessPartner partner)
        {
            if (ModelState.IsValid)
            {
                _context.Update(partner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partner);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var partner = await _context.BusinessPartners.FindAsync(id);
            if (partner == null) return NotFound();
            return View(partner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partner = await _context.BusinessPartners.FindAsync(id);
            if (partner != null)
            {
                _context.BusinessPartners.Remove(partner);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetData()
        {
            var partners = await _context.BusinessPartners
                .Select(bp => new
                {
                    bp.Id,
                    bp.Name,
                    bp.Category,
                    bp.EntityType
                })
                .ToListAsync();

            return Json(new { data = partners });
        }

    }
}
