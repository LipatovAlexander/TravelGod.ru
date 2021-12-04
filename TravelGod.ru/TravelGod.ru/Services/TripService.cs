using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Services
{
    public class TripService
    {
        private readonly ApplicationContext _context;

        public TripService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Trip>> GetTripsAsync(TripsOptions options)
        {
            var trips = from t in _context.Trips
                        select t;

            if (options.Status is not null)
            {
                trips = trips.Where(t => t.Status == options.Status);
            }

            if (!string.IsNullOrEmpty(options.Title))
            {
                trips = trips.Where(t => t.Title.ToLower().Contains(options.Title.ToLower()));
            }

            if (!string.IsNullOrEmpty(options.Dates))
            {
                var startDate = DateTime.ParseExact(options.Dates[..10], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact(options.Dates[13..23], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                trips = trips.Where(t => t.StartDate >= startDate && t.EndDate <= endDate);
            }

            if (!string.IsNullOrEmpty(options.Route))
            {
                var routes = options.Route.Split(new[] {' ', ';', ',', '-'}, StringSplitOptions.RemoveEmptyEntries);
                trips = routes.Aggregate(trips,
                    (current, route) => current.Where(t => t.RouteRaw.ToLower().Contains(route.ToLower())));
            }

            trips = (options.Archive
                        ? trips.Where(t => t.EndDate <= DateTime.Now)
                               .OrderByDescending(t => t.EndDate)
                        : trips.Where(t => t.EndDate > DateTime.Now)
                               .OrderBy(t => t.StartDate))
                    .Include(t => t.Initiator)
                    .ThenInclude(u => u.Avatar);

            return await PaginatedList<Trip>.CreateAsync(trips, options.PageNumber, TripsOptions.PageSize);
        }

        public async Task<List<Trip>> GetJoinedTripsAsync(int userId, int pageNumber, int pageSize, Status status = Status.Normal)
        {
            var trips = _context.Trips
                                .Include(t => t.Users)
                                .Where(t => t.Users.Any(u => u.Id == userId))
                                .Where(t => t.Status == status)
                                .OrderByDescending(t => t.EndDate);
            return await PaginatedList<Trip>.CreateAsync(trips, pageNumber, pageSize);
        }

        public async Task<Trip> GetTripAsync(int id, Status? status)
        {
            return await _context.Trips
                                     .Include(t => t.Users)
                                        .ThenInclude(u => u.Avatar)
                                     .Include(t => t.Chat)
                                     .Include(t => t.Chat)
                                         .ThenInclude(c => c.Messages)
                                     .Include(t => t.Comments)
                                         .ThenInclude(c => c.User)
                                         .ThenInclude(u => u.Avatar)
                                     .FirstOrDefaultAsync(t => t.Id == id && (status == null || t.Status == status));
        }

        public async Task AddTripAsync(Trip trip, User initiator)
        {
            trip.RouteRaw = string.Join(';', trip.RouteRaw
                                                 .Split(new[] {';', ','},
                                                     StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(t => t.Trim())
                                                 .Select(r => char.ToUpper(r[0]) + r[1..].ToLower()));
            trip.Initiator = initiator;
            trip.UsersCount = 1;
            trip.Users.Add(initiator);

            initiator.OwnedTripsCount += 1;
            initiator.JoinedTripsCount += 1;

            _context.Trips.Add(trip);
            _context.Users.Update(initiator);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToTrip(Trip trip, User user)
        {
            trip.Users.Add(user);
            trip.UsersCount += 1;
            user.JoinedTripsCount += 1;

            trip.Chat.Users.Add(user);
            _context.Trips.Update(trip);
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateTripAsync(Trip editedTrip)
        {
            _context.Trips.Update(editedTrip);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTripAsync(int id)
        {
            var trip = await _context.Trips
                               .Include(t => t.Users)
                                .ThenInclude(u => u.JoinedTrips)
                               .Include(t => t.Users)
                                .ThenInclude(u => u.JoinedChats)
                               .Include(t => t.Chat)
                                .ThenInclude(t => t.Messages)
                               .Include(t => t.Comments)
                               .FirstOrDefaultAsync(t => t.Id == id);
            if (trip is null)
            {
                throw new ArgumentException("Trip id doesn't match any exist trip.");
            }

            trip.Status = Status.RemovedByModerator;
            foreach (var comment in trip.Comments)
            {
                comment.Status = Status.RemovedByModerator;
            }
            _context.Comments.UpdateRange(trip.Comments);

            foreach (var user in trip.Users)
            {
                user.JoinedTripsCount -= 1;
            }

            trip.Initiator.OwnedTripsCount -= 1;
            if (trip.Chat is not null)
            {
                trip.Chat.Status = Status.RemovedByModerator;

                foreach (var message in trip.Chat.Messages)
                {
                    message.Status = Status.RemovedByModerator;
                }
                _context.Chats.Update(trip.Chat);
                _context.Messages.UpdateRange(trip.Chat.Messages);
            }

            _context.Users.UpdateRange(trip.Users);
            _context.Trips.Update(trip);

            await _context.SaveChangesAsync();
        }
    }
}