using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF.Data;
using SF.Models;

namespace SF.Controllers
{
    public class WITCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WITCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WITCategory wITCategory)
        {
            if (ModelState.IsValid)
            {
                _context.WITCategories.Add(wITCategory);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wITCategory);
        }

        public IActionResult Edit(int id)
        {
            var wit = _context.WITCategories.Find(id);
            if(wit == null)
            {
                return NotFound();
            }
            return View(wit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(WITCategory category)
        {
            if(ModelState.IsValid)
            {
                _context.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var wit = _context.WITCategories.Find(id);
            if(wit == null)
            {
                return NotFound();
            }
            return View(wit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var wit = _context.WITCategories.Find(id);
            if (wit == null)
            {
                return NotFound();
            }
            _context.WITCategories.Remove(wit);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult GetData()
        {
            var wit = _context.WITCategories
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    w.Code,
                    w.Detail,
                    w.GroupCode
                }).ToList();

            return Json(new { data = wit });
        }
    }
}
