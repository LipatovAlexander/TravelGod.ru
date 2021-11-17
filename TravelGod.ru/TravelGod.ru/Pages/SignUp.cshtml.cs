using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Infrastructure.Cryptography;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages
{
    public class SignUp : MyPageModel
    {
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

        public SignUp(ApplicationContext context) : base(context)
        {
        }

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

            if (_context.Users.FirstOrDefault(u => u.Login == Login) is not null)
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

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return RedirectToPage(nameof(SignIn));
        }
    }
}