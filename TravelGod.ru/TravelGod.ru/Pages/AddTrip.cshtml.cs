using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages
{
    public class AddTrip : MyPageModel
    {
        private readonly TripService _tripService;

        public AddTrip(TripService tripService)
        {
            _tripService = tripService;
        }

        [BindProperty] public Trip Trip { get; set; }
        [BindProperty] public bool CreateChat { get; set; }

        public IActionResult OnGet()
        {
            if (User is null)
            {
                return RedirectToPage("/SignIn");
            }

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
                    Users = new List<User>{User}
                };
            }

            await _tripService.AddTripAsync(Trip, User);
            return RedirectToPage("TripPage", new {id = Trip.Id});
        }
    }
}