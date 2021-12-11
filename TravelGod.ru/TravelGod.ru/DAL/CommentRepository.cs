using System.Threading.Tasks;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationContext context) : base(context)
        {
        }

        public void CreateFor(Trip trip, Comment comment)
        {
            comment.Trip = trip;
            Create(comment);
        }

        public async Task CreateForTripAsync(int tripId, Comment comment)
        {
            var trip = await Context.Trips.FindAsync(tripId);
            CreateFor(trip, comment);
        }
    }
}