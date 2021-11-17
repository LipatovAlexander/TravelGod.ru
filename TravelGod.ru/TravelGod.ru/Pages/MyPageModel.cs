using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages
{
    public abstract class MyPageModel : PageModel
    {
        public new User User => HttpContext.Items["User"] as User;
    }
}