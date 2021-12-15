using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;
using TravelGod.ru.Services.Filters;

namespace TravelGod.ru.Pages.Profile
{
    [AuthenticationPageFilter]
    [AllowSynchronousIo]
    public class Chats : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Chats(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Chat> ListOfChats { get; set; }

        [FromForm] public Message Message { get; set; }

        public async Task<IActionResult> OnGet()
        {
            ListOfChats = await _unitOfWork.Chats.GetAsync(c => c.Users.Contains(User) && c.Status == Status.Normal,
                chats => chats
                         .Include(c => c.Users.Where(u => u.Status == Status.Normal))
                         .ThenInclude(u => u.Avatar)
                         .Include(c => c.Messages.Where(m => m.Status == Status.Normal).OrderBy(m => m.CreatedAt))
                         .ThenInclude(m => m.CreatedBy.Avatar),
                chats => chats.OrderByDescending(c => c.ModifiedAt));
            return Page();
        }

        public async Task<IActionResult> OnPostSendMessage()
        {
            _unitOfWork.Messages.Create(Message);
            await _unitOfWork.SaveAsync();
            return ViewComponent("Message", new {Message});
        }

        public async Task<IActionResult> OnGetGetChat(int id)
        {
            var chat = await _unitOfWork.Chats.FirstOrDefaultAsync(
                c => c.Id == id && c.Status == Status.Normal,
                chats => chats
                         .Include(c => c.Messages.Where(m => m.Status == Status.Normal))
                         .ThenInclude(m => m.CreatedBy.Avatar));
            if (chat is null)
            {
                return BadRequest();
            }

            return ViewComponent("MessagesBox", new {Chat = chat});
        }
    }
}