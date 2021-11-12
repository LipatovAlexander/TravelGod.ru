using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages
{
    public class Profile : PageModel
    {
        public new User User { get; set; }

        public Profile(ApplicationContext context)
        {
            _context = context;
        }

        private readonly ApplicationContext _context;

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Request.Cookies.TryGetValue("token", out var token))
            {
                var session = _context.Sessions.FirstOrDefault(s => s.Token == token);
                if (session is not null && session.Expires > DateTimeOffset.Now)
                {
                    _context.Entry(session).Reference(s => s.User).Load();
                    User = session.User;
                    session.Expires = DateTimeOffset.Now.AddMinutes(20);
                    _context.Sessions.Update(session);
                    await _context.SaveChangesAsync();
                }
            }

            if (User is null)
            {
                return RedirectToPage(nameof(SignIn));
            }

            return Page();
        }
    }
}