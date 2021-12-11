using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationContext context) : base(context)
        {
        }

        public new void Create(Message item)
        {
            if (item.Chat is not null)
            {
                Context.Chats.Attach(item.Chat);
            }

            base.Create(item);
        }
    }
}