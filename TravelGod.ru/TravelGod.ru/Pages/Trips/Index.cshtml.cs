using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Pages.Trips
{
    public class Index : MyPageModel
    {
        private readonly TripService _tripService;

        public Index(TripService tripService)
        {
            _tripService = tripService;
        }

        public PaginatedList<Trip> ListOfTrips { get; private set; }
        [BindProperty(SupportsGet = true)] public TripsOptions Options { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ListOfTrips = await _tripService.GetTrips(Options);

            return Page();
        }
    }
}