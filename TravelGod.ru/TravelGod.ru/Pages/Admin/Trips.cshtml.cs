using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Pages.Admin.ViewModels;
using TravelGod.ru.Services;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Pages.Admin
{
    [AdministratorPageFilter]
    public class Trips : MyPageModel
    {
        private readonly TripService _tripService;

        public Trips(TripService tripService)
        {
            _tripService = tripService;
        }

        public PaginatedList<Trip> ListOfTrips { get; private set; }

        [BindProperty(SupportsGet = true)]
        public TripsOptions TripsOptions { get; set; }
        public Trip EditedTrip { get; set; }


        public async Task<IActionResult> OnGet()
        {
            TripsOptions.Status = null;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ListOfTrips = await _tripService.GetTripsAsync(TripsOptions);

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedTrip = await _tripService.GetTripAsync(id, null);

            if (EditedTrip is null)
            {
                return BadRequest();
            }

            if (!await TryUpdateModelAsync(EditedTrip, nameof(EditedTrip)))
            {
                return BadRequest();
            }

            ModelState.ClearValidationState(nameof(EditedTrip));
            if (!TryValidateModel(EditedTrip, nameof(EditedTrip)))
            {
                return BadRequest();
            }

            await _tripService.UpdateTripAsync(EditedTrip);
            return new JsonResult("success");
        }

        public async Task<IActionResult> OnGetRemove(int id)
        {
            try
            {
                await _tripService.RemoveTripAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return RedirectToPage("/Admin/Trips",
                new {archive = TripsOptions.Archive,
                    dates = TripsOptions.Dates,
                    route = TripsOptions.Route,
                    status = TripsOptions.Status,
                    pageNumber = TripsOptions.PageNumber});
        }
    }
}