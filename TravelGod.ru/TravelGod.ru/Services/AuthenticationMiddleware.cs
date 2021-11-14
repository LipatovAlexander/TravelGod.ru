using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationContext dbContext)
        {
            if (context.Request.Cookies.TryGetValue("token", out var token))
            {
                var session = await dbContext.Sessions.FirstOrDefaultAsync(s => s.Token == token);
                if (session is not null && session.Expires > DateTimeOffset.Now && token is not null)
                {
                    await dbContext.Entry(session).Reference(s => s.User).LoadAsync();
                    context.Items["User"] = session.User;
                    if (!session.RememberMe)
                    {
                        session.Expires = DateTimeOffset.Now.AddMinutes(20);
                    }
                    dbContext.Sessions.Update(session);
                    await dbContext.SaveChangesAsync();
                    context.Response.Cookies.Append("token", token,
                        new CookieOptions()
                        {
                            Expires = session.Expires,
                            Path = "/"
                        });
                    context.Items["Session"] = session;
                }
                else if (session is not null)
                {
                    dbContext.Sessions.Remove(session);
                    await dbContext.SaveChangesAsync();
                }
            }

            await _next(context);
        }
    }
}