using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        [BindProperty]
        public Trip Trip { get; set; }

        public IActionResult OnGet()
        {
            if (User is null)
            {
                Console.WriteLine(true);
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

            await _tripService.AddTripAsync(Trip, User);
            return RedirectToPage("TripPage", new {id = Trip.Id});
        }
    }
}