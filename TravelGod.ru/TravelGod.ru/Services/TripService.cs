using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Models;
using TravelGod.ru.Pages;

namespace TravelGod.ru.Services
{
    public class TripService
    {
        private readonly ApplicationContext _context;

        public TripService(ApplicationContext context)
        {
            _context = context;
        }

        public List<Trip> GetTrips(TripsSearchOptions options)
        {
            var result = _context.Trips
                                 .Where(t => !options.HasTitle ||
                                             EF.Functions.Like(t.Title.ToLower(),
                                                 $"%{options.Title.ToLower()}%"))
                                 .Where(t => !options.Archive || t.EndDate < DateTime.Now)
                                 .Where(t => !options.HasDates || t.StartDate >= options.StartDate &&
                                     t.EndDate <= options.EndDate)
                                 .AsEnumerable()
                                 .Where(t => !options.HasRoute || options.Route
                                                                         .All(route => t.Route
                                                                             .Any(routeItem =>
                                                                                 string.Equals(route, routeItem,
                                                                                     StringComparison
                                                                                         .InvariantCultureIgnoreCase))))
                                 .OrderBy(t=> t.StartDate)
                                 .ToList();

            return result;
        }

        public async Task<List<Trip>> GetJoinedTrips(int userId)
        {
            return await _context.Trips
                                 .Include(t => t.Users)
                                 .Where(t => t.Users.Any(u => u.Id == userId))
                                 .ToListAsync();

        }
    }
}