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
            var model = new UserCreateViewModel
            {
                Roles = await _roleManager.Roles
            .Select(r => new RoleViewModel
            {
                RoleName = r.Name,
                IsSelected = false
            })
            .ToListAsync()
            };

            ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
            ViewData["RoleGroups"] = new SelectList(await _context.RoleGroups.ToListAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model, int? RoleGroupId)
        {
            if (!ModelState.IsValid)
            {
                // Reload Roles and RoleGroups for the View
                model.Roles = await _roleManager.Roles
                    .Select(r => new RoleViewModel
                    {
                        RoleName = r.Name,
                        IsSelected = false
                    })
                    .ToListAsync();

                ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
                ViewData["RoleGroups"] = new SelectList(await _context.RoleGroups.ToListAsync(), "Id", "Name");
                return View(model);
            }

            // Create the user
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
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                // Reload Roles and RoleGroups for the View
                model.Roles = await _roleManager.Roles
                    .Select(r => new RoleViewModel
                    {
                        RoleName = r.Name,
                        IsSelected = false
                    })
                    .ToListAsync();

                ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
                ViewData["RoleGroups"] = new SelectList(await _context.RoleGroups.ToListAsync(), "Id", "Name");
                return View(model);
            }

            // Assign selected roles
            if (model.SelectedRoles != null && model.SelectedRoles.Any())
            {
                foreach (var roleName in model.SelectedRoles)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            // Assign roles from the selected RoleGroup
            if (RoleGroupId.HasValue)
            {
                var roleGroupRoles = await _context.RoleGroupRoles
                    .Where(rgr => rgr.RoleGroupId == RoleGroupId.Value)
                    .Select(rgr => rgr.RoleName)
                    .ToListAsync();

                foreach (var roleName in roleGroupRoles)
                {
                    // Avoid duplicate roles
                    if (!await _userManager.IsInRoleAsync(user, roleName))
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CompanyId = user.CompanyId,
                TelephoneNumber = user.TelephoneNumber,
                FaxNumber = user.FaxNumber,
                Roles = (await _roleManager.Roles.ToListAsync()).Select(role => new RoleViewModel
                {
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            ViewData["RoleGroups"] = new SelectList(await _context.RoleGroups.ToListAsync(), "Id", "Name");
            ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model, int? RoleGroupId)
        {
            if (!ModelState.IsValid)
            {
                // Reload dropdowns if validation fails
                ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
                ViewData["RoleGroups"] = new SelectList(await _context.RoleGroups.ToListAsync(), "Id", "Name");
                model.Roles = (await _roleManager.Roles.ToListAsync()).Select(role => new RoleViewModel
                {
                    RoleName = role.Name,
                    IsSelected = model.SelectedRoles.Contains(role.Name)
                }).ToList();
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

            // Handle roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.SelectedRoles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(model.SelectedRoles).ToList();

            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            if (rolesToAdd.Any())
            {
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }

            // Handle RoleGroup selection
            if (RoleGroupId.HasValue)
            {
                var roleGroupRoles = await _context.RoleGroupRoles
                    .Where(rgr => rgr.RoleGroupId == RoleGroupId.Value)
                    .Select(rgr => rgr.RoleName)
                    .ToListAsync();

                foreach (var roleName in roleGroupRoles)
                {
                    if (!await _userManager.IsInRoleAsync(user, roleName))
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                    }
                }
            }

            // Update the user
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            // Handle update errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // Reload dropdowns if there's an error
            ViewData["Companies"] = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
            ViewData["RoleGroups"] = new SelectList(await _context.RoleGroups.ToListAsync(), "Id", "Name");
            model.Roles = (await _roleManager.Roles.ToListAsync()).Select(role => new RoleViewModel
            {
                RoleName = role.Name,
                IsSelected = model.SelectedRoles.Contains(role.Name)
            }).ToList();

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

        // GET: User/ResetPassword/5
        //[Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult ResetPassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new ResetPasswordViewModel { UserId = id };
            return View(model);
        }

        // POST: User/ResetPassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                foreach (var error in removePasswordResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (addPasswordResult.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in addPasswordResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // GET: User/ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: User/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password changed successfully.";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


    }
}
