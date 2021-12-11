using System.Threading.Tasks;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        void CreateFor(Trip trip, Rating rating, User creator);
        Task CreateForTripAsync(int tripId, Rating rating, User creator);
    }
}