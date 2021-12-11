using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        public RatingRepository(ApplicationContext context) : base(context)
        {
        }

        public void CreateFor(Trip trip, Rating rating, User creator = null)
        {
            if (trip.Ratings is null)
            {
                Context.Entry(trip).Collection(t => t.Ratings).Load();
            }

            if (creator is not null)
            {
                if (trip.Users is null)
                {
                    Context.Entry(trip).Collection(t => t.Users).Load();
                }

                if (trip.Users is null || !trip.Users.Exists(u => u.Id == creator.Id))
                {
                    throw new Exception("Creator hasn't access to this action");
                }

                if (trip.Ratings is not null && trip.Ratings.Exists(r => r.CreatedById == creator.Id))
                {
                    throw new Exception("This creator already has left rating for this trip");
                }
            }

            trip.Ratings ??= new List<Rating>();
            trip.Ratings.Add(rating);
            var average = trip.Ratings.Select(r => (int) r.Point).Average();
            if (Math.Abs(average - trip.AverageRating) > 0.1)
            {
                trip.AverageRating = average;
            }

            Context.Trips.Update(trip);
        }

        public async Task CreateForTripAsync(int tripId, Rating rating, User creator = null)
        {
            var trip = creator is null
                ? await Context.Trips.FindAsync(tripId)
                : await Context.Trips
                               .Include(t => t.Users)
                               .Include(t => t.Ratings)
                               .FirstOrDefaultAsync(t => t.Id == tripId);
            CreateFor(trip, rating, creator);
        }
    }
}