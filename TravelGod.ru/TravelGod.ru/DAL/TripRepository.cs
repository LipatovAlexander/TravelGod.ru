using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.DAL
{
    public class TripRepository : GenericRepository<Trip>, ITripRepository
    {
        public TripRepository(ApplicationContext context) : base(context)
        {
        }

        public new void Create(Trip trip)
        {
            trip.RouteRaw = string.Join(';', trip.RouteRaw
                                                 .Split(new[] {';', ','},
                                                     StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(t => t.Trim())
                                                 .Select(r => char.ToUpper(r[0]) + r[1..].ToLower()));
            trip.EndDate = trip.EndDate.AddHours(23).AddMinutes(59);
            base.Create(trip);
        }

        public IEnumerable<Trip> Get(TripFilter filter,
                                     Func<IQueryable<Trip>, IIncludableQueryable<Trip, object>> include = null,
                                     Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = null)
        {
            return ApplyFilter(filter, include, orderBy).ToList();
        }

        public async Task<IEnumerable<Trip>> GetAsync(TripFilter filter,
                                                      Func<IQueryable<Trip>, IIncludableQueryable<Trip, object>>
                                                          include = null,
                                                      Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = null)
        {
            return await ApplyFilter(filter, include, orderBy).ToListAsync();
        }

        public async Task<PaginatedList<Trip>> GetPaginatedListAsync(int pageSize, int pageIndex, TripFilter filter,
                                                                     Func<IQueryable<Trip>,
                                                                             IIncludableQueryable<Trip, object>>
                                                                         include =
                                                                         null,
                                                                     Func<IQueryable<Trip>, IOrderedQueryable<Trip>>
                                                                         orderBy = null)
        {
            var source = ApplyFilter(filter, include, orderBy);
            return await PaginatedList<Trip>.CreateAsync(source, pageIndex, pageSize);
        }

        public async Task AddUserAsync(Trip trip, User user)
        {
            if (trip.Users is null)
            {
                await Context.Entry(trip).Collection(t => t.Users).LoadAsync();
                trip.Users ??= new List<User>();
            }

            trip.Users.Add(user);
            trip.UsersCount += 1;
            user.JoinedTripsCount += 1;
            trip.Chat?.Users.Add(user);
            Update(trip);
            Context.Users.Update(user);
        }

        public async Task AddUserAsync(int tripId, User user)
        {
            var trip = Context.Trips
                              .Include(t => t.Users)
                              .Include(t => t.Chat)
                              .FirstOrDefault(t => t.Id == tripId);
            if (trip is null)
            {
                throw new ArgumentException("Id doesn't match exists trip");
            }

            await AddUserAsync(trip, user);
        }

        private IQueryable<Trip> ApplyFilter(TripFilter filter,
                                             Func<IQueryable<Trip>, IIncludableQueryable<Trip, object>> include = null,
                                             Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = null)
        {
            var trips = Apply(null, include, orderBy);

            if (filter is null)
            {
                return trips;
            }

            if (filter.Status is not null)
            {
                trips = trips.Where(t => t.Status == filter.Status);
            }

            if (!string.IsNullOrEmpty(filter.Title))
            {
                trips = trips.Where(t => t.Title.ToLower().Contains(filter.Title.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Dates))
            {
                var startDate = DateTime.ParseExact(filter.Dates[..10], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact(filter.Dates[13..23], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                trips = trips.Where(t => t.StartDate >= startDate && t.EndDate <= endDate);
            }

            if (!string.IsNullOrEmpty(filter.Route))
            {
                var routes = filter.Route.Split(new[] {' ', ';', ',', '-'}, StringSplitOptions.RemoveEmptyEntries);
                trips = routes.Aggregate(trips,
                    (current, route) => current.Where(t => t.RouteRaw.ToLower().Contains(route.ToLower())));
            }

            trips = filter.Archive
                ? trips.Where(t => t.EndDate <= DateTime.Now)
                       .OrderByDescending(t => t.EndDate)
                : trips.Where(t => t.EndDate > DateTime.Now)
                       .OrderBy(t => t.StartDate);
            return trips;
        }
    }
}