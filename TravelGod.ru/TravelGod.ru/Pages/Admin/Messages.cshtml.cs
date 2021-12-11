using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages.Admin
{
    public class Messages : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Messages(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PaginatedList<Message> ListOfMessages { get; set; }
        public Message EditedMessage { get; set; }

        public async Task<IActionResult> OnGet(int pageIndex = 1)
        {
            const int pageSize = 10;
            ListOfMessages = await _unitOfWork.Messages.GetPaginatedListAsync(pageSize, pageIndex, null,
                messages => messages
                            .Include(m => m.CreatedBy)
                            .Include(m => m.Chat));

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedMessage = await _unitOfWork.Messages.FindByIdAsync(id);

            ModelState.Clear();
            if (EditedMessage is null
                || !await TryUpdateModelAsync(EditedMessage, nameof(EditedMessage))
                || !TryValidateModel(EditedMessage, nameof(EditedMessage)))
            {
                return BadRequest();
            }

            _unitOfWork.Messages.Update(EditedMessage);
            await _unitOfWork.SaveAsync();
            return new JsonResult("success");
        }

        public async Task<IActionResult> OnGetRemove(int id, int pageIndex)
        {
            var message = await _unitOfWork.Messages.FindByIdAsync(id);
            if (message?.Status is not Status.Normal)
            {
                return BadRequest();
            }

            message.Status = Status.RemovedByModerator;
            _unitOfWork.Messages.Update(message);
            await _unitOfWork.SaveAsync();

            return RedirectToPage("/Admin/Messages",
                new {pageIndex});
        }
    }
}