using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Pages.Admin.ViewModels;

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
            var user = await _context.Users
                                     .Include(u => u.Avatar)
                                     .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> GetUserAsync(string login)
        {
            var user = await _context.Users
                                     .Include(u => u.Avatar)
                                     .FirstOrDefaultAsync(u => u.Login == login);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user.AvatarId == 0)
            {
                user.AvatarId = 1;
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserAsync(User user)
        {
            user.AvatarId = 1;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<User>> GetUsersAsync(UsersOptions usersOptions)
        {
            var users = from user in _context.Users
                        select user;

            if (!string.IsNullOrEmpty(usersOptions.Name))
            {
                users = users.Where(u => u.Login.ToLower()
                                          .Contains(usersOptions.Name) ||
                                         (u.LastName + ' ' + u.FirstName + ' ' + u.Patronymic).ToLower()
                                         .Contains(usersOptions.Name));
            }

            users = users.Where(u => u.Role == usersOptions.Role &&
                                     u.Status == usersOptions.Status);

            users = users.OrderBy(u => u.Id)
                         .Include(u => u.Avatar);

            var paginatedList = await PaginatedList<User>.CreateAsync(users, usersOptions.PageNumber, UsersOptions.PageSize);

            return paginatedList;
        }
    }
}