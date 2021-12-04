using System;
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

        public async Task AddMessageAsync(Message message)
        {
            message.DateTime = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}