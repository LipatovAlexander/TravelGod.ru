using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages
{
    public class Trips : MyPageModel
    {
        private readonly FileService _fileService;
        private readonly TripService _tripService;
        private readonly UserService _userService;

        public Trips(TripService tripService, FileService fileService, UserService userService)
        {
            _tripService = tripService;
            _fileService = fileService;
            _userService = userService;
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

            foreach (var trip in ListOfTrips)
            {
                trip.Initiator = await _userService.GetUserAsync(trip.InitiatorId);
                trip.Initiator.Avatar = await _fileService.GetFileAsync(trip.Initiator.AvatarId);
            }

            return Page();
        }
    }
}