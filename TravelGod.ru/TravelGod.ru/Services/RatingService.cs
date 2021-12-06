using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class RatingService
    {
        private readonly ApplicationContext _context;

        public RatingService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddRatingAsync(Trip trip, Rating rating)
        {
            trip.Ratings.Add(rating);
            var average = trip.Ratings.Select(r => (int) r.Point).Average();
            if (Math.Abs(average - trip.AverageRating) > 0.1)
            {
                trip.AverageRating = average;
            }
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Rating>> GetRatingsAsync(int pageIndex)
        {
            const int pageSize = 10;
            var ratings = _context.Ratings
                                  .Include(r => r.User)
                                    .ThenInclude(u => u.Avatar)
                                  .Include(r => r.Trip);
            return await PaginatedList<Rating>.CreateAsync(ratings, pageIndex, pageSize);
        }

        public async Task<Rating> GetRatingAsync(int id, Status? status)
        {
            return await _context.Ratings
                                 .Include(r => r.Trip)
                                 .Include(r => r.User.Avatar)
                                 .FirstOrDefaultAsync(r => r.Id == id && (status == null || r.Status == status));
        }

        public async Task UpdateRatingAsync(Rating editedRating)
        {
            _context.Ratings.Update(editedRating);
            await _context.SaveChangesAsync();
        }
    }
}