using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services.Filters
{
    public class AdministratorPageFilter : Attribute, IAsyncPageFilter
    {
        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
                                                      PageHandlerExecutionDelegate next)
        {
            if (context.HttpContext.Items["User"] is User {Role: Role.Administrator})
            {
                await next();
            }
            else
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}