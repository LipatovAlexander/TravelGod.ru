using System.Threading.Tasks;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<Session> FindByTokenAsync(string token);
        Session CreateFor(User user, bool temporary);
        new void Remove(Session session);
    }
}