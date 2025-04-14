using ActivityClub.Data;
using ActivityClub.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    private readonly BOS _context;
    private readonly ILogger<AdminController> _logger;

    public AdminController(BOS context, ILogger<AdminController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult AdminLogin()
    {
        ViewData["Title"] = "Admin Login";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
    {
        ViewData["Title"] = "Admin Login";
        if (ModelState.IsValid)
        {
            var admin = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Pass == model.Password && u.UserRole == "Admin");
            if (admin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.UserName),
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("ManageAll", "Admin");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }

    


    // Create an event
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult CreateEvent()
    {
        ViewData["Title"] = "Create Event";
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEvent(Event newEvent)
    {
        if (ModelState.IsValid)
        {
            _logger.LogInformation("Creating a new event: {@Event}", newEvent);
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Event created successfully with ID: {EventID}", newEvent.EventID);
            return RedirectToAction("ManageAll");
        }
        else
        {
            _logger.LogWarning("Invalid model state for event: {@Event}", newEvent);
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
            }
        }
        return View(newEvent);
    }

    [Authorize(Roles = "Admin")]

    //admin can manage all actions
    public IActionResult ManageAll()
    {
        ViewData["Title"] = "Admin Management";

        try
        {
            var admins = _context.Users
                .Where(u => u.UserRole == "Admin")
                .ToList();

            var guides = _context.Guides.Select(g => new Guide
            {
                GuideID = g.GuideID,
                FullName = g.FullName ?? "No Name",
                Email = g.Email ?? "No Email",
                Password = g.Password ?? "No Password",
                DateOfBirth = g.DateOfBirth,
                JoiningDate = g.JoiningDate,
                Photo = g.Photo ?? new byte[0],
                Profession = g.Profession ?? "Unknown Profession"
            }).ToList();

            var events = _context.Events.ToList();

            var members = _context.Members.Select(m => new Member
            {
                MemberID = m.MemberID,
                FullName = m.FullName ?? "No Name",
                Email = m.Email ?? "No Email",
                MemberPassword = m.MemberPassword ?? "No Password",
                DateOfBirth = m.DateOfBirth,
                Gender = m.Gender,
                JoiningDate = m.JoiningDate,
                MobileNumber = m.MobileNumber ?? "No Mobile Number",
                EmergencyNumber = m.EmergencyNumber ?? "No Emergency Number",
                Photo = m.Photo ?? new byte[0],
                Profession = m.Profession ?? "Unknown Profession",
                Nationality = m.Nationality ?? "Unknown Nationality"
            }).ToList();

            var lookups = _context.Lookups.ToList();

            var viewModel = new AdminViewModel
            {
                Admins = admins,
                Guides = guides,
                Events = events,
                Members = members,
                Lookups = lookups
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            throw;
        }
    }


    //Edit Event
    [HttpGet]
    public async Task<IActionResult> EditEvent(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
        {
            return NotFound();
        }

        return View(eventItem);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEvent(int id, [Bind("EventID,EventName,Description,DateFrom,DateTo,OtherDetails")] Event eventItem)
    {
        if (id != eventItem.EventID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(eventItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventItem.EventID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("ManageAll");
        }
        return View(eventItem);
    }
    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.EventID == id);
    }


    //Delete Event
    [HttpPost]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var eventItem = await _context.Events.Include(e => e.EventMembers)
                                             .FirstOrDefaultAsync(e => e.EventID == id);

        if (eventItem == null)
        {
            return NotFound();
        }

        foreach (var member in eventItem.EventMembers.ToList())
        {
            _context.EventMembers.Remove(member);
        }

        _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();
        return RedirectToAction("ManageAll");
    }




    //Adding new admin
    [HttpGet]
    public IActionResult AddAdmin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAdmin(AdminViewModel model)
    {
        if (ModelState.IsValid)
        {
            var admin = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Pass = "Admin@123321", // default password set for new admins, they can change it later
                UserRole = "Admin"
            };

            _context.Users.Add(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageAll");
        }

        return View(model); 
    }

    //Edit admin
    [HttpGet]
    public async Task<IActionResult> EditAdmin(int? id)
    {
        if (id == null) return NotFound();

        var admin = await _context.Users.FindAsync(id);
        if (admin == null) return NotFound();

        var model = new AdminViewModel
        {
            UserID = admin.UserID,
            UserName = admin.UserName,
            Email = admin.Email,
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAdmin(AdminViewModel model)
    {
        if (ModelState.IsValid)
        {
            var admin = await _context.Users.FindAsync(model.UserID);
            if (admin == null) return NotFound();

            admin.UserName = model.UserName;
            admin.Email = model.Email;

            _context.Update(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageAll"); 
        }
        return View(model);
    }

    //Delete admin
    [HttpPost, ActionName("DeleteAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var admin = await _context.Users.FindAsync(id);
        if (admin == null) return NotFound();

        var eventMemberships = _context.EventMembers.Where(em => em.MemberID == id);
        _context.EventMembers.RemoveRange(eventMemberships);

        _context.Users.Remove(admin);
        await _context.SaveChangesAsync();
        return RedirectToAction("ManageAll");
    }





    //Add guide
    [HttpGet]
    public IActionResult AddGuide()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddGuide(Guide model)
    {
        model.Password = model.Password ?? "Guide@123321";

        _context.Guides.Add(model);
        await _context.SaveChangesAsync();

        var user = new User
        {
            UserName = model.FullName,
            Email = model.Email,
            Pass = model.Password,
            UserRole = "Guide"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("ManageAll");
    }

    //Edit guide
    [HttpGet]
    public async Task<IActionResult> EditGuide(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var guide = await _context.Guides.FindAsync(id);
        if (guide == null)
        {
            return NotFound();
        }

        return View(guide);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditGuide(int id, [Bind("GuideID,FullName,Email,Password")] Guide guide)
    {
        if (id != guide.GuideID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(guide);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GuideExists(guide.GuideID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ManageAll));
        }
        return View(guide);
    }

    private bool GuideExists(int id)
    {
        return _context.Guides.Any(e => e.GuideID == id);
    }

    //Delete guide
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteGuide(int id)
    {
        var guide = await _context.Guides.FindAsync(id);
        if (guide != null)
        {
            _context.Guides.Remove(guide);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("ManageAll");
    }




    

    public IActionResult AdminDashboard()
    {
        ViewData["Title"] = "Admin Dashboard";
        return View();
    }
}
