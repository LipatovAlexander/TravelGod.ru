using System;
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
                item.Chat.ModifiedAt = DateTime.Now;
                Context.Chats.Attach(item.Chat);
                Context.Chats.Update(item.Chat);
            }

            base.Create(item);
        }
    }
}