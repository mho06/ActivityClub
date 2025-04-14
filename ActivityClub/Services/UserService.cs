using ActivityClub.Models;
using ActivityClub.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActivityClub.Services
{
    public class UserService : IUserService
    {
        private readonly BOS _context;
        private readonly ILogger<UserService> _logger;

        public UserService(BOS context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Pass == password);
            if (user != null)
                return user;

            var guide = await _context.Guides.FirstOrDefaultAsync(g => g.Email == email && g.Password == password);
            if (guide != null)
            {
                return new User
                {
                    UserID = guide.GuideID, 
                    UserName = guide.FullName,
                    Email = guide.Email,
                    UserRole = "Guide",
                    Pass = guide.Password 
                };
            }

            return null;
        }



        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users
                .Include(u => u.EventMembers)
                .ThenInclude(em => em.Event)
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
        }


        public async Task CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
