using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class CommentService
    {
        private readonly ApplicationContext _context;

        public CommentService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Comment>> GetCommentsAsync(int pageIndex)
        {
            const int pageSize = 10;
            var comments = _context.Comments
                                 .Include(c => c.Trip)
                                 .Include(c => c.User)
                                 .ThenInclude(u => u.Avatar);
            return await PaginatedList<Comment>.CreateAsync(comments, pageIndex, pageSize);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Comment> GetCommentAsync(int id, Status? status)
        {
            return await _context.Comments
                           .Include(c => c.Trip)
                           .Include(c => c.User)
                                .ThenInclude(u => u.Avatar)
                           .FirstOrDefaultAsync(c => c.Id == id && (status == null || c.Status == status));
        }

        public async Task UpdateCommentAsync(Comment editedComment)
        {
            _context.Comments.Update(editedComment);
            await _context.SaveChangesAsync();
        }
    }
}