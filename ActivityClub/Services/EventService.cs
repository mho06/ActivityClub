using ActivityClub.Data;
using ActivityClub.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActivityClub.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task CreateEventAsync(Event eventModel);
        Task RegisterUserForEventAsync(int eventId, int userId);
        Task AddGuideToEventAsync(int eventId, int guideId);
        Task CancelUserRegistrationForEventAsync(int eventId, int userId);

    }

    public class EventService : IEventService
    {
        private readonly BOS _context;

        public EventService(BOS context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.EventMembers)
                .Include(e => e.EventGuides)
                .ToListAsync();
        }

        public async Task CreateEventAsync(Event eventModel)
        {
            _context.Events.Add(eventModel);
            await _context.SaveChangesAsync();
        }

        public async Task RegisterUserForEventAsync(int eventId, int userId)
        {
            var existingRegistration = await _context.EventMembers
                .FirstOrDefaultAsync(em => em.EventID == eventId && em.MemberID == userId);

            if (existingRegistration != null)
            {
                throw new Exception("You are already registered for this event.");
            }

            var eventMember = new EventMembers { EventID = eventId, MemberID = userId };
            _context.EventMembers.Add(eventMember);
            await _context.SaveChangesAsync();
        }

        public async Task CancelUserRegistrationForEventAsync(int eventId, int userId)
        {
            var eventMember = await _context.EventMembers
                                            .FirstOrDefaultAsync(em => em.EventID == eventId && em.MemberID == userId);

            if (eventMember != null)
            {
                _context.EventMembers.Remove(eventMember);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("You are not registered for this event.");
            }
        }



        public async Task AddGuideToEventAsync(int eventId, int guideId)
        {
            var eventGuide = new EventGuides { EventID = eventId, GuideID = guideId };
            _context.EventGuides.Add(eventGuide);
            await _context.SaveChangesAsync();
        }
    }
}
