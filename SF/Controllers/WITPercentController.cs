using Microsoft.AspNetCore.Mvc;
using SF.Data;
using SF.Models;

namespace SF.Controllers
{
    public class WITPercentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WITPercentController(ApplicationDbContext context)
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
        public IActionResult Create(WITPercent wITPercent)
        {
            if(ModelState.IsValid)
            {
                _context.WITPercents.Add(wITPercent);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wITPercent);
        }

        public IActionResult Edit(int id)
        {
            var wit = _context.WITPercents.Find(id);
            if(wit == null)
            {
                return NotFound();
            }
            return View(wit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(WITPercent wITPercent)
        {
            if (ModelState.IsValid)
            {
                _context.Update(wITPercent);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wITPercent);
        }

        public IActionResult Delete(int id)
        {
            var wit = _context.WITPercents.Find(id);
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
            var wit = _context.WITPercents.Find(id);
            if (wit == null)
            {
                return NotFound();
            }

            _context.WITPercents.Remove(wit);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult GetData()
        {
            var wit = _context.WITPercents.ToList();

            return Json( new { data = wit });
        }
    }
}
