using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages.Trips
{
    [AuthenticationPageFilter]
    public class Add : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Add(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty] public Trip Trip { get; set; }
        [BindProperty] public bool CreateChat { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _unitOfWork.Trips.Create(Trip);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.Trips.AddUserAsync(Trip, User);
            User.OwnedTripsCount += 1;
            _unitOfWork.Users.Update(User);
            await _unitOfWork.SaveAsync();

            if (CreateChat)
            {
                await _unitOfWork.Chats.CreateForAsync(Trip, User);
                await _unitOfWork.SaveAsync();
            }

            return RedirectToPage("/Trips/Concrete", new {id = Trip.Id});
        }
    }
}