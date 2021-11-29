using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages.Trips
{
    [AuthenticationPageFilter]
    public class Add : MyPageModel
    {
        private readonly TripService _tripService;

        public Add(TripService tripService)
        {
            _tripService = tripService;
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

            if (CreateChat)
            {
                Trip.Chat = new Chat
                {
                    Initiator = User,
                    Name = Trip.Title,
                    Users = new List<User>{User},
                    IsGroupChat = true
                };
            }

            await _tripService.AddTripAsync(Trip, User);
            return RedirectToPage("/Trips/Concrete", new {id = Trip.Id});
        }
    }
}