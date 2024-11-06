using Microsoft.AspNetCore.Mvc;
using SF.Services;

namespace SF.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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
