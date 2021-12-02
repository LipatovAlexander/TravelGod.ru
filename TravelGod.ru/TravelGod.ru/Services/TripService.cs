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
        private readonly ChatService _chatService;
        private readonly ApplicationContext _context;
        private readonly UserService _userService;
        private readonly FileService _fileService;
        private readonly CommentService _commentService;

        public TripService(ApplicationContext context, ChatService chatService, UserService userService, FileService fileService, CommentService commentService)
        {
            _context = context;
            _chatService = chatService;
            _userService = userService;
            _fileService = fileService;
            _commentService = commentService;
        }

        public async Task<PaginatedList<Trip>> GetTrips(TripsOptions options, Status status = Status.Normal)
        {
            var trips = from t in _context.Trips
                        select t;

            trips = trips.Where(t => t.Status == status);

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

            trips = options.Archive
                ? trips.Where(t => t.EndDate <= DateTime.Now)
                       .OrderByDescending(t => t.EndDate)
                : trips.Where(t => t.EndDate > DateTime.Now)
                       .OrderBy(t => t.StartDate);

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

        public async Task<Trip> GetTripAsync(int id, Status status = Status.Normal)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip is null || trip.Status != status)
            {
                return null;
            }

            await _context.Entry(trip).Collection(t => t.Users).LoadAsync();
            await _context.Entry(trip).Reference(t => t.Chat).LoadAsync();
            foreach (var user in trip.Users)
            {
                user.Avatar = await _fileService.GetFileAsync(user.AvatarId);
            }

            trip.Comments = await _commentService.GetCommentsAsync(trip);

            return trip;
        }

        private async Task AddTripAsync(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
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

            await AddTripAsync(trip);
            initiator.OwnedTripsCount += 1;
            initiator.JoinedTripsCount += 1;
            await _userService.UpdateUserAsync(initiator);
        }

        public async Task AddUserToTrip(Trip trip, User user)
        {
            trip.Users.Add(user);
            trip.UsersCount += 1;
            user.JoinedTripsCount += 1;

            _context.Trips.Update(trip);
            await _chatService.AddUserToChat(trip.Chat, user);
            await _userService.UpdateUserAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}