using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Message>> GetMessagesAsync(Chat chat, Status status)
        {
            return _context.Messages
                           .Where(m => m.Chat.Id == chat.Id)
                           .Where(m => m.Status == status)
                           .OrderBy(m => m.DateTime)
                           .ToList();
        }

        public async Task AddMessageAsync(Message message)
        {
            message.DateTime = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}