﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Comment>> GetCommentsAsync(Trip trip, Status status = Status.Normal)
        {
            return await _context.Comments
                                 .Include(c => c.Trip)
                                 .Where(c => c.Trip == trip)
                                 .Where(c => c.Status == status)
                                 .Include(c => c.User)
                                 .ThenInclude(u => u.Avatar)
                                 .OrderByDescending(c => c.Date)
                                 .ToListAsync();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
    }
}