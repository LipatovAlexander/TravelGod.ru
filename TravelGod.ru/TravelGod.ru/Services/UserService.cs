using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Areas.Admin.ViewModels;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class UserService
    {
        private readonly ApplicationContext _context;
        private readonly FileService _fileService;

        public UserService(ApplicationContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is not null)
            {
                user.Avatar = await _fileService.GetFileAsync(user.AvatarId);
            }
            return user;
        }

        public async Task<User> GetUserAsync(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user is not null)
            {
                user.Avatar = await _fileService.GetFileAsync(user.AvatarId);
            }
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            if (await _fileService.GetFileAsync(user.AvatarId) is null)
            {
                user.Avatar = await _fileService.GetFileAsync(1);
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserAsync(User user)
        {
            user.Avatar = await _fileService.GetFileAsync(1);
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

            users = users.OrderBy(u => u.Id);

            var paginatedList = await PaginatedList<User>.CreateAsync(users, usersOptions.PageNumber, UsersOptions.PageSize);

            foreach (var user in paginatedList)
            {
                user.Avatar = await _fileService.GetFileAsync(user.AvatarId);
            }

            return paginatedList;
        }
    }
}