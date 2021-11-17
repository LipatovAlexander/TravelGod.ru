using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        public async Task InvokeAsync(HttpContext context, SessionService sessionService, UserService userService)
        {
            if (context.Request.Cookies.TryGetValue("token", out var token))
            {
                var session = await sessionService.GetSessionAsync(token);
                if (session is not null && session.Expires > DateTimeOffset.Now && token is not null)
                {
                    session.User = await userService.GetUserAsync(session.UserId);
                    if (session.User.Status != Status.Normal)
                    {
                        await sessionService.RemoveSessionAsync(session);
                    }
                    else
                    {
                        context.Items["User"] = session.User;
                        if (!session.RememberMe)
                        {
                            session.Expires = DateTimeOffset.Now.AddMinutes(20);
                        }

                        await sessionService.UpdateSessionAsync(session);
                        context.Response.Cookies.Append("token", token,
                            new CookieOptions
                            {
                                Expires = session.Expires,
                                Path = "/"
                            });
                        context.Items["Session"] = session;
                    }
                }
                else if (session is not null)
                {
                    await sessionService.RemoveSessionAsync(session);
                }
            }

            await _next(context);
        }
    }
}