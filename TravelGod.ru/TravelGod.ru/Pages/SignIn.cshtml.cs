using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages
{
    public class SignIn : MyPageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        [BindProperty] public bool RememberMe { get; set; }

        public SignIn(ApplicationContext context) : base(context)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (User is not null)
            {
                return RedirectToPage(nameof(Profile));
            }

            var user = _context.Users.FirstOrDefault(u => u.Login == Login);
            if (user is null)
            {
                ModelState.AddModelError("Password", "Неправильный логин или пароль");
                return Page();
            }

            var actualPasswordHash = Cryptography.ComputeMd5HashString(Password + user.PasswordSalt);

            Console.WriteLine(RememberMe);
            if (actualPasswordHash == user.PasswordHash)
            {
                var accessToken = Cryptography.GenerateRandomCryptographicKey(40);
                var session = new Session
                {
                    RememberMe = RememberMe,
                    Expires = RememberMe ? DateTimeOffset.Now.AddYears(1) : DateTimeOffset.Now.Add(TimeSpan.FromMinutes(20)),
                    Token = accessToken,
                    User = user
                };
                HttpContext.Response.Cookies.Append("token", accessToken,
                    new CookieOptions()
                    {
                        Expires = session.Expires,
                        Path = "/"
                    });
                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToPage(nameof(Profile), new {id = user.Id});
            }

            ModelState.AddModelError("Password", "Неправильный логин или пароль");
            return Page();
        }
    }
}