using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.DAL.Interfaces;
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
                return RedirectToPage("/Profile/Index", new{id = User.Id});
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (User is not null)
            {
                return RedirectToPage("/Profile/Index", new{id = User.Id});
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Login == SignInModel.Login);
                _unitOfWork.Sessions.CreateFor(user, !SignInModel.RememberMe);
                await _unitOfWork.SaveAsync();
                return RedirectToPage("/Profile/Index", new {id = user.Id});
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}