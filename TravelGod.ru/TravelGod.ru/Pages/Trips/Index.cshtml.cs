using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Pages.Trips
{
    public class Index : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Index(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PaginatedList<Trip> ListOfTrips { get; private set; }
        [BindProperty(SupportsGet = true)] public TripFilter Filter { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            const int pageSize = 10;
            ListOfTrips = await _unitOfWork.Trips.GetPaginatedListAsync(pageSize, Filter.PageNumber, Filter,
                trips => trips
                    .Include(t => t.CreatedBy.Avatar));

            return Page();
        }
    }
}