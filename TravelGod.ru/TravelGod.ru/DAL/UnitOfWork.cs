using System;
using System.Threading.Tasks;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationContext _context;

        private bool _disposed;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Chats = new ChatRepository(context);
            Comments = new CommentRepository(context);
            Files = new FileRepository(context);
            Messages = new MessageRepository(context);
            Ratings = new RatingRepository(context);
            Sessions = new SessionRepository(context);
            Trips = new TripRepository(context);
            Avatars = new AvatarRepository(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IUserRepository Users { get; }
        public IChatRepository Chats { get; }
        public ICommentRepository Comments { get; }
        public IFileRepository Files { get; }
        public IMessageRepository Messages { get; }
        public IRatingRepository Ratings { get; }
        public ISessionRepository Sessions { get; }
        public ITripRepository Trips { get; }
        public IAvatarRepository Avatars { get; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }
    }
}