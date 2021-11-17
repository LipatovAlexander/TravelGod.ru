using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class UserService
    {
        private readonly ApplicationContext _context;

        public UserService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            await _context.Entry(user).Collection(u => u.JoinedTrips).LoadAsync();
            await _context.Entry(user).Reference(u => u.Avatar).LoadAsync();
            return user;
        }

        public async Task<User> GetUserAsync(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            await _context.Entry(user).Collection(u => u.JoinedTrips).LoadAsync();
            await _context.Entry(user).Reference(u => u.Avatar).LoadAsync();
            return user;
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync();
        }
    }
}