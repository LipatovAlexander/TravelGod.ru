using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public List<Trip> ListOfTrips { get; set; }

        [BindProperty(SupportsGet = true)] public TripsSearchOptions SearchOptions { get; set; }

        public async Task OnGet()
        {
            if (!ModelState.IsValid)
            {
                ListOfTrips = new List<Trip>();
                return;
            }

            ListOfTrips = _tripService.GetTrips(SearchOptions);
            foreach (var trip in ListOfTrips)
            {
                trip.Initiator = await _userService.GetUserAsync(trip.InitiatorId);
                trip.Initiator.Avatar = await _fileService.GetFileAsync(trip.Initiator.AvatarId);
            }
        }
    }
}