using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class AuthenticationPageFilter : Attribute, IAsyncPageFilter
    {
        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
                                                      PageHandlerExecutionDelegate next)
        {
            if (context.HttpContext.Items["User"] is User)
            {
                await next();
            }
            else
            {
                context.Result = new RedirectToPageResult("/SignIn");
            }
        }
    }
}