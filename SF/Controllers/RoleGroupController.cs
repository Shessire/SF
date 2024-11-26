using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF.Data;
using SF.Models;

namespace SF.Controllers
{
    public class RoleGroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleGroupController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roleGroups = await _context.RoleGroups.Include(rg => rg.RoleGroupRoles).ToListAsync();
            return View(roleGroups);
        }

        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            // Fetch role groups
            var roleGroups = await _context.RoleGroups.ToListAsync();

            // Pass role groups and roles to the view
            ViewData["Roles"] = roles;
            ViewData["RoleGroups"] = roleGroups;

            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string groupName, List<string> selectedRoles)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                ModelState.AddModelError("", "Group name is required.");
                return View();
            }

            if (!selectedRoles.Any())
            {
                ModelState.AddModelError("", "At least one role must be selected.");
                return View();
            }

            var roleGroup = new RoleGroup { Name = groupName };
            _context.RoleGroups.Add(roleGroup);
            await _context.SaveChangesAsync();

            foreach (var role in selectedRoles)
            {
                _context.RoleGroupRoles.Add(new RoleGroupRoles { RoleGroupId = roleGroup.Id, RoleName = role });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var roleGroup = await _context.RoleGroups
                .Include(rg => rg.RoleGroupRoles)
                .FirstOrDefaultAsync(rg => rg.Id == id);

            if (roleGroup == null)
                return NotFound();

            return View(roleGroup);
        }
    }
}
