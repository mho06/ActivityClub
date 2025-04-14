using ActivityClub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActivityClub.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string pass);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User?> GetUserByEmail(string email);
        Task<User> GetUserByUsername(string username);
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }
}
