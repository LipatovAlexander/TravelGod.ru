using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            user.Avatar = await _fileService.GetFileAsync(user.AvatarId);
            return user;
        }

        public async Task<User> GetUserAsync(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            user.Avatar = await _fileService.GetFileAsync(user.AvatarId);
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
    }
}