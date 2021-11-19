using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Infrastructure.Cryptography;
using TravelGod.ru.Services;
using TravelGod.ru.ViewModels;

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

        [BindProperty] public SignInModel SignInModel { get; set; }

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

            var user = await _userService.GetUserAsync(SignInModel.Login);
            if (user is null)
            {
                ModelState.AddModelError("SignInModel.Password", "Неправильный логин или пароль");
                return Page();
            }

            var actualPasswordHash = Cryptography.ComputeMd5HashString(SignInModel.Password + user.PasswordSalt);

            if (actualPasswordHash == user.PasswordHash)
            {
                var session = await _sessionService.AddSessionAsync(user, !SignInModel.RememberMe);
                HttpContext.Response.Cookies.Append("token", session.Token,
                    new CookieOptions
                    {
                        Expires = session.Expires,
                        Path = "/"
                    });
                return RedirectToPage(nameof(Profile), new {id = user.Id});
            }

            ModelState.AddModelError("SignInModel.Password", "Неправильный логин или пароль");
            return Page();
        }
    }
}