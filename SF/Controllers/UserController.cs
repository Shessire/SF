using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SF.Data;
using SF.Models;
using SF.Services;
using SF.ViewModel;

namespace SF.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUserService userService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Include(u => u.Company).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
            ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CompanyId = model.CompanyId,
                    TelephoneNumber = model.TelephoneNumber,
                    FaxNumber = model.FaxNumber
            };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Re-populate the role and company lists if model validation fails
            ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
            ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");

            return View(model);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the user by ID
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Map user data to the UserEditViewModel
            var model = new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CompanyId = user.CompanyId,
                TelephoneNumber = user.TelephoneNumber,
                FaxNumber = user.FaxNumber,
                RoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault() // Get current role
            };

            // Load companies and roles into ViewData for dropdowns
            ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
            ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload dropdowns if validation fails
                ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
                ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.CompanyId = model.CompanyId;
            user.TelephoneNumber = model.TelephoneNumber;
            user.FaxNumber = model.FaxNumber;

            // Check and update role
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (model.RoleName != currentRoles.FirstOrDefault())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.RoleName);
            }

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var passwordResult = await _userManager.RemovePasswordAsync(user);
                if (passwordResult.Succeeded)
                {
                    var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                    if (!addPasswordResult.Succeeded)
                    {
                        foreach (var error in addPasswordResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
                        ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
                        return View(model);
                    }
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            // Handle update errors
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // Reload dropdowns if there's an error
            ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
            ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
            return View(model);
        }


        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.Users
                                          .Include(u => u.Company)
                                          .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(user);
        }

    }
}
