using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF.Data;
using SF.Models;

namespace SF.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Create
        public IActionResult Create()
        {
            ViewBag.BaseUOMs = new[] { "UNT", "CRB", "JOB", "H", "Day" };
            ViewBag.Types = new[] { "Product", "Service" };
            ViewBag.Categories = new[] { "Purchase", "Sale", "Consignment" };
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.BaseUOMs = new[] { "UNT", "CRB", "JOB", "H", "Day" };
            ViewBag.Types = new[] { "Product", "Service" };
            ViewBag.Categories = new[] { "Purchase", "Sale", "Consignment" };
            return View(item);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            ViewBag.BaseUOMs = new[] { "UNT", "CRB", "JOB", "H", "Day" };
            ViewBag.Types = new[] { "Product", "Service" };
            ViewBag.Categories = new[] { "Purchase", "Sale", "Consignment" };
            return View(item);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.BaseUOMs = new[] { "UNT", "CRB", "JOB", "H", "Day" };
            ViewBag.Types = new[] { "Product", "Service" };
            ViewBag.Categories = new[] { "Purchase", "Sale", "Consignment" };
            return View(item);
        }

        // POST: Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetData()
        {
            var items = await _context.Items
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.Type,
                    i.Category,
                    i.Price,
                    i.Cost
                }).ToListAsync();

            return Json(new { data = items });
        }
    }
}
