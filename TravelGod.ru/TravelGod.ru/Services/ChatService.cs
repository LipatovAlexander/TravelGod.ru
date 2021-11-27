using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class ChatService
    {
        private readonly ApplicationContext _context;
        private readonly FileService _fileService;
        private readonly MessageService _messageService;

        public ChatService(ApplicationContext context, FileService fileService, MessageService messageService)
        {
            _context = context;
            _fileService = fileService;
            _messageService = messageService;
        }

        public async Task AddChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Chat>> GetChatsAsync(User user, Status status = Status.Normal)
        {
            var chats = _context.Chats
                                .Include(c => c.Users)
                                .Where(c => c.Users.Contains(user))
                                .Where(c => c.Status == status)
                                .ToList();
            foreach (var chat in chats)
            {
                chat.Messages = await _messageService.GetMessagesAsync(chat, status);
                foreach (var chatUser in chat.Users)
                {
                    chatUser.Avatar = await _fileService.GetFileAsync(chatUser.AvatarId);
                }
            }

            var groupChats = chats
                             .Where(c => c.IsGroupChat)
                             .OrderByDescending(c =>
                                 c.Messages.FirstOrDefault() != null
                                     ? c.Messages.FirstOrDefault().DateTime
                                     : DateTime.MinValue);
            var personalChats = chats
                                .Where(c => !c.IsGroupChat)
                                .OrderByDescending(c =>
                                    c.Messages.FirstOrDefault() != null
                                        ? c.Messages.FirstOrDefault().DateTime
                                        : DateTime.MinValue);

            return groupChats.Concat(personalChats).ToList();
        }

        public async Task<Chat> GetChatAsync(int chatId)
        {
            var chat = await _context.Chats.FindAsync(chatId);
            await _context.Entry(chat).Reference(c => c.Initiator).LoadAsync();
            await _context.Entry(chat).Collection(c => c.Messages).LoadAsync();
            await _context.Entry(chat).Collection(c => c.Users).LoadAsync();
            foreach (var chatUser in chat.Users)
            {
                chatUser.Avatar = await _fileService.GetFileAsync(chatUser.AvatarId);
            }

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
    }
}