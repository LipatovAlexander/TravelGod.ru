using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Infrastructure.Cryptography;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages
{
    public class SignUp : MyPageModel
    {
        private readonly UserService _userService;

        public SignUp(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Введите логин")]
        [MinLength(5, ErrorMessage = "Логин не должен быть короче 5 символов")]
        [MaxLength(10, ErrorMessage = "Логин не должен быть длиннее 10 символов")]
        [RegularExpression(@"^[A-Za-z0-9]*$", ErrorMessage = "Логин должен состоять из букв и цифр")]
        public string Login { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(5, ErrorMessage = "Пароль не должен быть короче 5 символов")]
        [MaxLength(10, ErrorMessage = "Пароль не должен быть длиннее 10 символов")]
        [RegularExpression(@"^[A-Za-z0-9]{5,10}$", ErrorMessage = "Пароль должен состоять из букв и цифр")]
        public string Password1 { get; set; }

        [BindProperty]
        [Compare(nameof(Password1), ErrorMessage = "Пароли должны совпадать")]
        public string Password2 { get; set; }

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

            if (await _userService.GetUserAsync(Login) is not null)
            {
                ModelState.AddModelError("Login", "Логин уже занят");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var passwordSalt = Cryptography.GenerateRandomCryptographicKey(32);
            var passwordHash = Cryptography.ComputeMd5HashString(Password1 + passwordSalt);

            var newUser = new User
            {
                Login = Login,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userService.AddUserAsync(newUser);
            return RedirectToPage(nameof(SignIn));
        }
    }
}