using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class TripService
    {
        private readonly ApplicationContext _context;
        private readonly ChatService _chatService;

        public TripService(ApplicationContext context, ChatService chatService)
        {
            _context = context;
            _chatService = chatService;
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
                                 .OrderBy(t => t.StartDate)
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

        public async Task<Trip> GetTripAsync(int id)
        {
            return await _context.Trips.FindAsync(id);
        }

        public async Task AddTripAsync(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
        }
        
        public async Task AddTripAsync(Trip trip, string routeRaw, User initiator)
        {
            trip.Route = routeRaw
                             .Split(new[] {',', ' ', '-'}, StringSplitOptions.RemoveEmptyEntries)
                             .Select(r => char.ToUpper(r[0]) + r[1..].ToLower())
                             .ToList();

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