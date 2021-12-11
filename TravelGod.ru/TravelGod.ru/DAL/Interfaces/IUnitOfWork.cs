using System.Threading.Tasks;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IChatRepository Chats { get; }
        ICommentRepository Comments { get; }
        IFileRepository Files { get; }
        IMessageRepository Messages { get; }
        IRatingRepository Ratings { get; }
        ISessionRepository Sessions { get; }
        ITripRepository Trips { get; }
        IAvatarRepository Avatars { get; }

        Task SaveAsync();
    }
}