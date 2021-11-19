using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages
{
    public class TripPage : MyPageModel
    {
        private readonly TripService _tripService;

        public TripPage(TripService tripService)
        {
            _tripService = tripService;
        }

        public Trip Trip { get; set; }

        public IActionResult OnGet(int id)
        {
            Trip = _tripService.GetTripAsync(id).Result;
            if (Trip is null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}