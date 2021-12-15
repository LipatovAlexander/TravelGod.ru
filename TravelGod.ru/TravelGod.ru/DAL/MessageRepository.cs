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
            item.Chat = Context.Chats.Find(item.Chat.Id);
            if (item.Chat is not null)
            {
                item.Chat.ModifiedAt = DateTime.Now;
                Context.Chats.Update(item.Chat);
            }
            else
            {
                throw new ArgumentException("Message.Chat.Id doesn't match any exists chat");
            }

            base.Create(item);
        }
    }
}