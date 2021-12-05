using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class MessageService
    {
        private readonly ApplicationContext _context;

        public MessageService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(Message message)
        {
            message.DateTime = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Message>> GetMessagesAsync(int pageIndex)
        {
            const int pageSize = 10;
            var messages = _context.Messages
                                   .Include(m => m.User)
                                   .ThenInclude(u => u.Avatar)
                                   .Include(m => m.Chat);
            return await PaginatedList<Message>.CreateAsync(messages, pageIndex, pageSize);
        }

        public async Task<Message> GetMessageAsync(int id, Status? status)
        {
            return await _context.Messages
                                 .Include(m => m.Chat)
                                 .Include(m => m.User)
                                 .ThenInclude(u => u.Avatar)
                                 .FirstOrDefaultAsync(m => m.Id == id && (status == null || m.Status == status));
        }

        public async Task UpdateMessageAsync(Message editedMessage)
        {
            _context.Messages.Update(editedMessage);
            await _context.SaveChangesAsync();
        }
    }
}