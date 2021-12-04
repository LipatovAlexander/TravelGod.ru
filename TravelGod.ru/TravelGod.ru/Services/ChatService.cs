using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Infrastructure;
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

        public async Task<List<Chat>> GetChatsAsync(User user, Status? status)
        {
            var chats = await _context.Chats
                                      .Include(c => c.Users.Where(u => u.Status == Status.Normal))
                                      .ThenInclude(u => u.Avatar)
                                      .Include(c => c.Messages.Where(m => m.Status == Status.Normal))
                                      .Where(c => c.Users.Contains(user))
                                      .Where(c => status == null || c.Status == status)
                                      .ToListAsync();

            var groupChats = chats
                             .Where(c => c.IsGroupChat)
                             .OrderByDescending(c =>
                                 c.Messages.FirstOrDefault()?.DateTime ?? DateTime.MinValue);
            var personalChats = chats
                                .Where(c => !c.IsGroupChat)
                                .OrderByDescending(c =>
                                    c.Messages.FirstOrDefault()?.DateTime ?? DateTime.MinValue);

            return groupChats.Concat(personalChats).ToList();
        }

        public async Task<PaginatedList<Chat>> GetChatsAsync(int pageIndex)
        {
            const int pageSize = 10;
            var chats = _context.Chats
                                .Include(c => c.Users)
                                .ThenInclude(u => u.Avatar)
                                .Include(c => c.Messages)
                                .Include(c => c.Initiator)
                                .ThenInclude(u => u.Avatar);
            return await PaginatedList<Chat>.CreateAsync(chats, pageIndex, pageSize);
        }

        public async Task<Chat> GetChatAsync(int chatId, Status? status)
        {
            var chat = await _context.Chats
                                     .Include(c => c.Users.Where(u => u.Status == Status.Normal))
                                     .ThenInclude(u => u.Avatar)
                                     .Include(c => c.Messages.Where(m => m.Status == Status.Normal))
                                     .FirstOrDefaultAsync(c => c.Id == chatId && (status == null || c.Status == status));

            return chat;
        }

        public async Task CreateChatForTripAsync(Trip trip)
        {
            var chat = new Chat
            {
                Initiator = trip.Initiator,
                Name = trip.Title,
                Users = trip.Users,
                IsGroupChat = true
            };
            trip.Chat = chat;
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateChatAsync(Chat editedChat)
        {
            _context.Chats.Update(editedChat);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveChatAsync(Chat chat)
        {
            chat.Status = Status.RemovedByModerator;
            foreach (var message in chat.Messages)
            {
                message.Status = Status.RemovedByModerator;
            }

            _context.Chats.Update(chat);
            _context.Messages.UpdateRange(chat.Messages);
            await _context.SaveChangesAsync();
        }
    }
}