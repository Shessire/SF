using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF.Data;
using SF.Models;

namespace SF.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var projects = _context.Projects.ToList();
            return View(projects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Add(project);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public IActionResult Edit (int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Project project)
        {
            if( id != project.Id )
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(project);
        }

        public IActionResult Delete(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed (int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            _context.Projects.Remove(project);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult GetData()
        {
            var projects = _context.Projects
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Type,
                    p.Description,
                    StartDate = p.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = p.EndDate.ToString("yyyy-MM-dd")
                }).ToList();

            return Json(new { data = projects });
        }
    }
}
