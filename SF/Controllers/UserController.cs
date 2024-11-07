using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF.Models;
using SF.Services;

namespace SF.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Include(u => u.Company).ToListAsync();
            return View(users);
        }


        //GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        //POST: User/AssignCompany
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignCompany(string userId, int companyId)
        {
            if (await _userService.AssignUserToCompanyAsync(userId, companyId))
            {
                return RedirectToAction(nameof(Details), new { id = userId });
            }

            // If assignment failed
            return BadRequest("Failed to assign user to company");
        }
    }
}
