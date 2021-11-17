using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class SessionService
    {
        private readonly ApplicationContext _context;

        public SessionService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Session> GetSessionAsync(int id)
        {
            return await _context.Sessions.FindAsync(id);
        }

        public async Task<Session> GetSessionAsync(string token)
        {
            return await _context.Sessions.FirstOrDefaultAsync(s => s.Token == token);
        }

        public async Task RemoveSessionAsync(Session session)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSessionAsync(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task AddSessionAsync(Session session)
        {
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
        }
    }
}