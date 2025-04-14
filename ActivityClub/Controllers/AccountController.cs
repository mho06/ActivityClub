using Microsoft.AspNetCore.Mvc;
using ActivityClub.Models;
using ActivityClub.Services;
using System;
using ActivityClub.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace ActivityClub.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly BOS _context;

        public AccountController(IUserService userService, ILogger<AccountController> logger, BOS context)
        {
            _userService = userService;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(User model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userService.GetUserByEmail(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email already exists. Would you like to log in instead?");
                    return View(model);
                }

                var existingUsername = await _userService.GetUserByUsername(model.UserName);
                if (existingUsername != null)
                {
                    ModelState.AddModelError("", "Username already exists. Please choose a different username.");
                    return View(model);
                }

                try
                {
                    model.UserRole = "User";
                    await _userService.CreateUser(model);
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating user");
                    ModelState.AddModelError("", "Error creating user: " + ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string pass, string returnUrl = null)
        {
            var user = await _userService.Authenticate(email, pass);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("FullName", user.UserName),
                    new Claim(ClaimTypes.Role, user.UserRole),
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                Response.Cookies.Append("auth", "true");

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("auth");
            return RedirectToAction("Index", "Home");
        }

        [Route("Account/IsLoggedIn")]
        [HttpGet]
        public IActionResult IsLoggedIn()
        {
            var isLoggedIn = User.Identity.IsAuthenticated;
            return Json(new { isLoggedIn });
        }
    }
}
