using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Infrastructure.Cryptography;
using TravelGod.ru.Models;
using TravelGod.ru.Services;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Pages.Profile
{
    public class SignUp : MyPageModel
    {
        private readonly UserService _userService;

        public SignUp(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty] public SignUpModel SignUpModel { get; set; }

        public IActionResult OnGet()
        {
            if (User is not null)
            {
                return RedirectToPage("/Profile/Index", new {id = User.Id});
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (User is not null)
            {
                return RedirectToPage("/Profile/Index", new {id = User.Id});
            }

            if (await _userService.GetUserAsync(SignUpModel.Login, Status.Normal) is not null)
            {
                ModelState.AddModelError("SignUpModel.Login", "Логин уже занят");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var passwordSalt = Cryptography.GenerateRandomCryptographicKey(32);
            var passwordHash = Cryptography.ComputeMd5HashString(SignUpModel.Password1 + passwordSalt);

            var newUser = new User
            {
                Login = SignUpModel.Login,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userService.AddUserAsync(newUser);
            return RedirectToPage("/Profile/SignIn");
        }
    }
}