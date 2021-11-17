using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Infrastructure.Cryptography;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages
{
    public class SignIn : MyPageModel
    {
        private readonly SessionService _sessionService;

        private readonly UserService _userService;

        public SignIn(UserService userService, SessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        [BindProperty] public bool RememberMe { get; set; }

        public IActionResult OnGet()
        {
            if (User is not null)
            {
                return RedirectToPage(nameof(Profile));
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (User is not null)
            {
                return RedirectToPage(nameof(Profile));
            }

            var user = await _userService.GetUserAsync(Login);
            if (user is null)
            {
                ModelState.AddModelError("Password", "Неправильный логин или пароль");
                return Page();
            }

            var actualPasswordHash = Cryptography.ComputeMd5HashString(Password + user.PasswordSalt);

            if (actualPasswordHash == user.PasswordHash)
            {
                var accessToken = Cryptography.GenerateRandomCryptographicKey(40);
                var session = new Session
                {
                    RememberMe = RememberMe,
                    Expires = RememberMe
                        ? DateTimeOffset.Now.AddYears(1)
                        : DateTimeOffset.Now.Add(TimeSpan.FromMinutes(20)),
                    Token = accessToken,
                    User = user
                };
                HttpContext.Response.Cookies.Append("token", accessToken,
                    new CookieOptions
                    {
                        Expires = session.Expires,
                        Path = "/"
                    });
                await _sessionService.AddSessionAsync(session);
                return RedirectToPage(nameof(Profile), new {id = user.Id});
            }

            ModelState.AddModelError("Password", "Неправильный логин или пароль");
            return Page();
        }
    }
}