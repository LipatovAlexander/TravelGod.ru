using System.Threading.Tasks;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        void CreateFor(Trip trip, Comment comment);
        Task CreateForTripAsync(int tripId, Comment comment);
    }
}