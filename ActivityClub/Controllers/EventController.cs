using Microsoft.AspNetCore.Mvc;
using ActivityClub.Models;
using ActivityClub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using ActivityClub.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ActivityClub.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly BOS _context;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventService eventService, BOS context, ILogger<EventController> logger)
        {
            _eventService = eventService;
            _context = context;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.Lookups.ToList();
            ViewBag.Categories = new SelectList(categories, "LookupID", "LookupName");
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> EIndex()
        {
            var events = await _context.Events
        .Include(e => e.EventGuides)  
        .ThenInclude(eg => eg.Guide) 
        .Select(e => new EventViewModel
        {
            EventID = e.EventID,
            EventName = e.EventName,
            EventDes = e.EventDes,
            Destination = e.Destination,
            DateFrom = e.DateFrom,
            DateTo = e.DateTo,
            Cost = e.Cost,
            Stat = e.Stat,
            CategoryLookupID = e.CategoryLookupID ?? 0,
            LookupID = e.LookupID ?? 0,
            EventGuides = e.EventGuides.ToList() 
        }).ToListAsync();

            var guides = await _context.Guides.ToListAsync();

            ViewBag.Guides = guides;
            return View(events);
        }





        //Join an event
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Register(int eventId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            try
            {
                await _eventService.RegisterUserForEventAsync(eventId, userId);
                TempData["SuccessMessage"] = "You have successfully joined the event.";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already registered"))
                {
                    TempData["ErrorMessage"] = "You are already registered for this event.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while registering for the event. Please try again later.";
                }
                
            }
            return RedirectToAction("EIndex");
        }

        //Cancel registration
        [Authorize]
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
            return RedirectToAction("EIndex");
        }

        //shows the details of the guide
        [HttpGet]
        public async Task<IActionResult> GuideDetails(int id)
        {
            var guide = await _context.Guides.FindAsync(id);
            if (guide == null)
            {
                return NotFound();
            }
            return View(guide);
        }




        
    }
}
