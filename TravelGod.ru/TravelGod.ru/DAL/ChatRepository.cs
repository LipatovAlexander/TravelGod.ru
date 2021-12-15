using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        public ChatRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task CreateForAsync(Trip trip, User creator = null)
        {
            await Context.Entry(trip).Reference(t => t.Chat).LoadAsync();
            if (trip.Chat is not null)
            {
                throw new Exception("This trip already has chat");
            }

            if (creator is not null && trip.CreatedById != creator.Id)
            {
                throw new Exception("Creator hasn't access to this action");
            }

            if (trip.Users is null)
            {
                await Context.Entry(trip).Collection(t => t.Users).LoadAsync();
            }

            trip.Chat = new Chat
            {
                Name = trip.Title,
                Users = trip.Users,
                IsGroupChat = true
            };
            Create(trip.Chat);
        }

        public async Task CreateForTripAsync(int tripId, User creator = null)
        {
            var trip = await Context.Trips
                                    .Include(t => t.Users)
                                    .FirstOrDefaultAsync(t => t.Id == tripId);
            if (trip is null)
            {
                throw new ArgumentException("Id doesn't match exists trip");
            }

            await CreateForAsync(trip, creator);
        }

        public async Task AddUserAsync(Chat chat, User user)
        {
            if (chat.Users is null)
            {
                await Context.Entry(chat).Collection(c => c.Users).LoadAsync();
            }

            chat.Users ??= new List<User>();
            chat.Users.Add(user);
            Update(chat);
        }
    }
}