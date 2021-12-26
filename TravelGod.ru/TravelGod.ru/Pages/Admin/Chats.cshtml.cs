using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages.Admin
{
    public class Chats : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Chats(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PaginatedList<Chat> ListOfChats { get; set; }
        public Chat EditedChat { get; set; }

        public async Task<IActionResult> OnGet(int pageIndex = 1)
        {
            const int pageSize = 10;
            ListOfChats = await _unitOfWork.Chats.GetPaginatedListAsync(pageSize, pageIndex, null,
                chats => chats.Include(c => c.CreatedBy)
                              .Include(c => c.Users));

            return Page();
        }

        public async Task<IActionResult> OnPostRemove(int id)
        {
            var chat = await _unitOfWork.Chats.FindByIdAsync(id);
            if (chat?.Status is not Status.Normal)
            {
                return BadRequest();
            }

            chat.Status = Status.RemovedByModerator;
            _unitOfWork.Chats.Update(chat);
            await _unitOfWork.SaveAsync();

            return new OkResult();
        }
    }
}