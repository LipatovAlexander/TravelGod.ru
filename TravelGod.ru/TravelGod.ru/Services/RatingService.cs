using System.Threading.Tasks;
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

        public async Task AddRatingAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
        }
    }
}