using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ActivityClub.Models;
using ActivityClub.Services;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace ActivityClub.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IUserService userService, IEventService eventService, ILogger<ProfileController> logger)
        {
            _userService = userService;
            _eventService = eventService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);

            if (email == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetUserByEmail(email);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int eventId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            try
            {
                await _eventService.CancelUserRegistrationForEventAsync(eventId, userId);
                TempData["SuccessMessage"] = "You have successfully canceled your registration for the event.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while canceling your registration for the event. Please try again later.";
            }
            return RedirectToAction("Index");
        }


        //update user profile 
        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userService.GetUserById(user.UserID);
                if (existingUser != null)
                {
                    existingUser.UserName = user.UserName;
                    existingUser.DateOfBirth = user.DateOfBirth;
                    existingUser.Gender = user.Gender;
                    existingUser.Email = user.Email;

                    existingUser.Pass = user.Pass;
                    existingUser.UserRole = user.UserRole;
                    existingUser.ConfirmPassword = user.Pass; 
                    existingUser.EventGuides = user.EventGuides;

                    await _userService.UpdateUser(existingUser);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "User not found.");
                }
            }

            // Log model state errors for debugging
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError(error.ErrorMessage);
            }

            return View(user);
        }
    }
}
