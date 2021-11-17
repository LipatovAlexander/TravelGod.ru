using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages
{
    public class Trips : MyPageModel
    {
        public List<Trip> ListOfTrips { get; set; }

        [BindProperty(SupportsGet = true)]
        public TripsSearchOptions SearchOptions { get; set; }

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

        public Trips(TripService tripService, FileService fileService, UserService userService)
        {
            _tripService = tripService;
            _fileService = fileService;
            _userService = userService;
        }

        private readonly UserService _userService;
        private readonly TripService _tripService;
        private readonly FileService _fileService;
    }
}