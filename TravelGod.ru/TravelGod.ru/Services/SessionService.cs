using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Infrastructure.Cryptography;
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
            var session = await _context.Sessions
                                        .Include(s => s.User)
                                        .ThenInclude(u => u.Avatar)
                                        .FirstOrDefaultAsync(s => s.Token == token);
            return session;
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

        public async Task<Session> AddSessionAsync(User user, bool temporary)
        {
            var accessToken = Cryptography.GenerateRandomCryptographicKey(40);
            var session = new Session
            {
                RememberMe = !temporary,
                Expires = !temporary
                    ? DateTimeOffset.Now.AddYears(1)
                    : DateTimeOffset.Now.Add(TimeSpan.FromMinutes(20)),
                Token = accessToken,
                User = user
            };
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }
    }
}