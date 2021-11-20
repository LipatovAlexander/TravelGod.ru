using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages
{
    public class TripPage : MyPageModel
    {
        private readonly TripService _tripService;
        private readonly FileService _fileService;

        public TripPage(TripService tripService, FileService fileService)
        {
            _tripService = tripService;
            _fileService = fileService;
        }

        public Trip Trip { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Trip = await _tripService.GetTripAsync(id);
            if (Trip is null)
            {
                return NotFound();
            }

            Trip.Initiator.Avatar = await _fileService.GetFileAsync(Trip.Initiator.AvatarId);
            return Page();
        }

        public async Task<IActionResult> OnGetJoin(int id)
        {
            Trip = await _tripService.GetTripAsync(id);
            if (User is null || Trip is null || Trip.Users.Contains(User))
            {
                return BadRequest();
            }

            await _tripService.AddUserToTrip(Trip, User);
            return Page();
        }
    }
}