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

        public TripService(ApplicationContext context, ChatService chatService)
        {
            _context = context;
            _chatService = chatService;
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

            trips = options.Archive
                ? trips.Where(t => t.EndDate <= DateTime.Now)
                : trips.Where(t => t.EndDate > DateTime.Now);

            if (!string.IsNullOrEmpty(options.Route))
            {
                var routes = options.Route.Split(new[] {' ', ';', ',', '-'}, StringSplitOptions.RemoveEmptyEntries);
                trips = routes.Aggregate(trips,
                    (current, route) => current.Where(t => t.RouteRaw.ToLower().Contains(route.ToLower())));
            }

            trips = trips.OrderBy(t => t.StartDate);

            return await PaginatedList<Trip>.CreateAsync(trips, options.PageNumber, TripsOptions.PageSize);
        }

        public async Task<List<Trip>> GetJoinedTripsAsync(int userId)
        {
            return await _context.Trips
                                 .Include(t => t.Users)
                                 .Where(t => t.Users.Any(u => u.Id == userId))
                                 .OrderByDescending(t => t.EndDate)
                                 .ToListAsync();
        }

        public async Task<Trip> GetTripAsync(int id)
        {
            return await _context.Trips.FindAsync(id);
        }

        public async Task AddTripAsync(Trip trip)
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
        }
    }
}