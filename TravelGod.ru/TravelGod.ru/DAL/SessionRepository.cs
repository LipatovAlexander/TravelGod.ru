using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure.Cryptography;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<Session> FindByTokenAsync(string token)
        {
            return await FirstOrDefaultAsync(
                s => s.Token == token,
                sessions => sessions
                            .Include(s => s.User)
                            .ThenInclude(u => u.Avatar));
        }

        public Session CreateFor(User user, bool temporary)
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
            Create(session);
            return session;
        }
    }
}