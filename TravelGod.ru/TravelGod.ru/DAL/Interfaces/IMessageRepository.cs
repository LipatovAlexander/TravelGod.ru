using TravelGod.ru.Models;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        new void Create(Message item);
    }
}