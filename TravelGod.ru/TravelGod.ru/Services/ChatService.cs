using System.Threading.Tasks;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class ChatService
    {
        private readonly ApplicationContext _context;

        public ChatService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }
    }
}