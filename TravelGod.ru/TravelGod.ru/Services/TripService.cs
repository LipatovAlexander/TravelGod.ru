using System;
using System.Collections.Generic;
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

        public async Task<PaginatedList<Trip>> GetTrips(TripsOptions options)
        {
            var trips = from t in _context.Trips
                        select t;

            if (!string.IsNullOrEmpty(options.Title))
            {
                trips = trips.Where(t => t.Title.ToLower().Contains(options.Title.ToLower()));
            }

            if (!string.IsNullOrEmpty(options.Dates))
            {
                var startDate = DateTime.Parse(options.Dates[..10]);
                var endDate = DateTime.Parse(options.Dates[13..23]);
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

        public async Task<List<Trip>> GetJoinedTripsAsync(int userId, int pageNumber, int pageSize)
        {
            var trips = _context.Trips
                                .Include(t => t.Users)
                                .Where(t => t.Users.Any(u => u.Id == userId))
                                .OrderByDescending(t => t.EndDate);
            return await PaginatedList<Trip>.CreateAsync(trips, pageNumber, pageSize);
        }

        public async Task<Trip> GetTripAsync(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip is null)
            {
                return null;
            }

            await _context.Entry(trip).Collection(t => t.Users).LoadAsync();
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
                                                 .Split(new[] {' ', ';', '-', ','},
                                                     StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(r => char.ToUpper(r[0]) + r[1..].ToLower()));
            trip.Initiator = initiator;
            trip.UsersCount = 1;
            trip.Users.Add(initiator);
            if (trip.CreateChat)
            {
                trip.Chat = new Chat
                {
                    Initiator = initiator,
                    Name = trip.Title,
                    CreationDate = DateTime.Now
                };
                trip.Chat.Users.Add(initiator);
                await _chatService.AddChatAsync(trip.Chat);
            }

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
            await _userService.UpdateUserAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}