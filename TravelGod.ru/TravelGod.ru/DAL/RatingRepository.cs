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

                if (trip.Ratings?.FirstOrDefault(r => r.CreatedById == creator.Id && r.Status == Status.Normal) != null)
                {
                    throw new Exception("This creator already has left rating for this trip");
                }
            }

            trip.Ratings ??= new List<Rating>();
            trip.Ratings.Add(rating);
            UpdateRating(trip);

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

        public async Task RemoveAsync(Rating rating)
        {
            if (rating.Status is not Status.Normal)
            {
                throw new InvalidOperationException("This rating has already been removed");
            }

            rating.Status = Status.RemovedByModerator;

            if (rating.Trip is null)
            {
                await Context.Entry(rating).Reference(r => r.Trip).LoadAsync();
            }

            if (rating.Trip is not null)
            {
                if (rating.Trip.Ratings is null)
                {
                    await Context.Entry(rating.Trip).Collection(t => t.Ratings).LoadAsync();
                }

                if (rating.Trip.Ratings is not null)
                {
                    UpdateRating(rating.Trip);
                }
            }
        }

        public async Task RemoveAsync(int id)
        {
            var rating = await FirstOrDefaultAsync(
                r => r.Id == id,
                ratings => ratings
                    .Include(r => r.Trip.Ratings));

            if (rating is null)
            {
                throw new ArgumentException("Id doesn't match any exists rating");
            }

            await RemoveAsync(rating);
        }

        public new void Remove(Rating rating)
        {
            RemoveAsync(rating).GetAwaiter().GetResult();
        }

        private void UpdateRating(Trip trip)
        {
            var average = trip.Ratings
                              .Where(r => r.Status == Status.Normal)
                              .Select(r => (int) r.Point)
                              .Average();
            if (Math.Abs(average - trip.AverageRating) > 0.1)
            {
                trip.AverageRating = average;
                Context.Trips.Update(trip);
            }
        }
    }
}