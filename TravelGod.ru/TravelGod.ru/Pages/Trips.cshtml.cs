using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages
{
    public class Trips : MyPageModel
    {
        public List<Trip> ListOfTrips { get; set; }

        [BindProperty(SupportsGet = true)]
        public TripsSearchOptions SearchOptions { get; set; }

        public void OnGet()
        {
            if (!ModelState.IsValid)
            {
                ListOfTrips = new List<Trip>();
                return;
            }

            var trips = _context.Trips
                                 .Include(t => t.Users)
                                 .ThenInclude(u => u.Avatar)
                                 .Include(t => t.Initiator)
                                 .Where(t => !SearchOptions.HasTitle ||
                                             EF.Functions.Like(t.Title.ToLower(),
                                                 $"%{SearchOptions.Title.ToLower()}%"))
                                 .Where(t => !SearchOptions.Archive || t.EndDate < DateTime.Now)
                                 .Where(t => !SearchOptions.HasDates || t.StartDate >= SearchOptions.StartDate &&
                                     t.EndDate <= SearchOptions.EndDate)
                                 .AsEnumerable();
            if (SearchOptions.HasRoute)
            {
                trips = trips
                    .Where(trip => SearchOptions.Route
                                                .All(route => trip.Route
                                                                  .Any(routeItem =>
                                                                      string.Equals(route, routeItem,
                                                                          StringComparison
                                                                              .InvariantCultureIgnoreCase))));
            }

            ListOfTrips = trips
                          .OrderBy(t => t.StartDate)
                          .ToList();
        }

        public Trips(ApplicationContext context) : base(context)
        {
        }
    }
}