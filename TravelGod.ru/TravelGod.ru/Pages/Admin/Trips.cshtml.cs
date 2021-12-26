using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services.Filters;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Pages.Admin
{
    [AdministratorPageFilter]
    public class Trips : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Trips(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PaginatedList<Trip> ListOfTrips { get; private set; }

        [BindProperty(SupportsGet = true)] public TripFilter TripFilter { get; set; }

        public Trip EditedTrip { get; set; }


        public async Task<IActionResult> OnGet()
        {
            TripFilter.Status = null;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            const int pageSize = 10;
            ListOfTrips = await _unitOfWork.Trips.GetPaginatedListAsync(pageSize, TripFilter.PageNumber, TripFilter,
                trips => trips.Include(t => t.CreatedBy));

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedTrip = await _unitOfWork.Trips.FirstOrDefaultAsync(t => t.Id == id,
                trips => trips.Include(t => t.CreatedBy));

            ModelState.Clear();
            if (EditedTrip is null
                || !await TryUpdateModelAsync(EditedTrip, nameof(EditedTrip))
                || !TryValidateModel(EditedTrip, nameof(EditedTrip)))
            {
                return BadRequest();
            }

            _unitOfWork.Trips.Update(EditedTrip);
            await _unitOfWork.SaveAsync();
            return new JsonResult("success");
        }



        public async Task<IActionResult> OnPostRemove(int id)
        {
            var trip = await _unitOfWork.Trips.FindByIdAsync(id);
            if (trip?.Status is not Status.Normal)
            {
                return BadRequest();
            }

            trip.Status = Status.RemovedByModerator;
            _unitOfWork.Trips.Update(trip);
            await _unitOfWork.SaveAsync();

            return new OkResult();
        }
    }
}