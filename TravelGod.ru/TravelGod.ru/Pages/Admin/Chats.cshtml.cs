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
                chats => chats.Include(c => c.CreatedBy));

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedChat = await _unitOfWork.Chats.FindByIdAsync(id);

            ModelState.Clear();
            if (EditedChat is null
                || !await TryUpdateModelAsync(EditedChat, nameof(EditedChat))
                || !TryValidateModel(EditedChat, nameof(EditedChat)))
            {
                return BadRequest();
            }

            _unitOfWork.Chats.Update(EditedChat);
            await _unitOfWork.SaveAsync();
            return new JsonResult("success");
        }

        public async Task<IActionResult> OnGetRemove(int id, int pageIndex)
        {
            var chat = await _unitOfWork.Chats.FirstOrDefaultAsync(c => c.Id == id && c.Status == Status.Normal);
            if (chat is null)
            {
                return BadRequest();
            }

            chat.Status = Status.RemovedByModerator;
            _unitOfWork.Chats.Update(chat);
            await _unitOfWork.SaveAsync();

            return RedirectToPage("/Admin/Chats",
                new {pageIndex});
        }
    }
}