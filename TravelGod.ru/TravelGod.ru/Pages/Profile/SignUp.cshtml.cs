using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Pages.Profile
{
    public class SignUp : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public SignUp(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _unitOfWork.Users.Create(SignUpModel);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest();
            }

            return RedirectToPage("/Profile/SignIn");
        }
    }
}