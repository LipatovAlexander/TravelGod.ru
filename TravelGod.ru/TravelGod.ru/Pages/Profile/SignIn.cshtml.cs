using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure.Cryptography;
using TravelGod.ru.Models;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Pages.Profile
{
    public class SignIn : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public SignIn(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Login == SignInModel.Login);
            if (user is null)
            {
                ModelState.AddModelError("SignInModel.Password", "Неправильный логин или пароль");
                return Page();
            }

            if (user.Status is not Status.Normal)
            {
                ModelState.AddModelError("SignInModel.Login", "Аккаунт заблокирован.");
                return Page();
            }

            var actualPasswordHash = Cryptography.ComputeMd5HashString(SignInModel.Password + user.PasswordSalt);

            if (actualPasswordHash == user.PasswordHash)
            {
                var session = _unitOfWork.Sessions.CreateFor(user, !SignInModel.RememberMe);
                await _unitOfWork.SaveAsync();
                HttpContext.Response.Cookies.Append("token", session.Token,
                    new CookieOptions
                    {
                        Expires = session.Expires,
                        Path = "/"
                    });
                return RedirectToPage("/Profile/Index", new {id = user.Id});
            }

            ModelState.AddModelError("SignInModel.Password", "Неправильный логин или пароль");
            return Page();
        }
    }
}