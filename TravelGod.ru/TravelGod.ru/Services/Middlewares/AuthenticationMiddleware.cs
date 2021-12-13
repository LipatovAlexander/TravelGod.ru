using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            if (context.Request.Cookies.TryGetValue("token", out var token))
            {
                var session = await unitOfWork.Sessions.FindByTokenAsync(token);
                if (session is not null && session.Expires > DateTimeOffset.Now && token is not null)
                {
                    if (session.User.Status != Status.Normal)
                    {
                        unitOfWork.Sessions.Remove(session);
                        await unitOfWork.SaveAsync();
                    }
                    else
                    {
                        context.Items["User"] = session.User;
                        if (!session.RememberMe)
                        {
                            session.Expires = DateTimeOffset.Now.AddMinutes(20);
                            unitOfWork.Sessions.Update(session);
                            await unitOfWork.SaveAsync();
                        }

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
                    unitOfWork.Sessions.Remove(session);
                    await unitOfWork.SaveAsync();
                }
            }

            await _next(context);
        }
    }
}