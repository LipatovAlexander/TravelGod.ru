using System.Threading.Tasks;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        Task CreateForAsync(Trip trip, User creator = null);
        Task CreateForTripAsync(int tripId, User creator = null);
        Task AddUserAsync(Chat chat, User user);
    }
}