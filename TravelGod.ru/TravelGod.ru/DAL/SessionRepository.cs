using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure.Cryptography;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly IHttpContextAccessor _accessor;

        public SessionRepository(ApplicationContext context, IHttpContextAccessor accessor) : base(context)
        {
            _accessor = accessor;
        }

        public new void Remove(Session session)
        {
            _accessor.HttpContext?.Response.Cookies.Append("token", session.Token,
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(-1)
                });

            base.Remove(session);
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

            _accessor.HttpContext?.Response.Cookies.Append("token", session.Token,
                new CookieOptions
                {
                    Expires = session.Expires,
                    Path = "/"
                });

            return session;
        }
    }
}