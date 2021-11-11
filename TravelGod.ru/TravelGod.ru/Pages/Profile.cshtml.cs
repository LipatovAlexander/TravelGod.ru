using System;
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

        public IActionResult OnGet()
        {
            if (User is null)
            {
                return RedirectToPage(nameof(SignIn));
            }

            return Page();
        }
    }
}