using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages.Admin
{
    public class Messages : MyPageModel
    {
        private readonly MessageService _messageService;

        public Messages(MessageService messageService)
        {
            _messageService = messageService;
        }

        public PaginatedList<Message> ListOfMessages { get; set; }
        public Message EditedMessage { get; set; }

        public async Task<IActionResult> OnGet(int pageIndex = 1)
        {
            ListOfMessages = await _messageService.GetMessagesAsync(pageIndex);

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedMessage = await _messageService.GetMessageAsync(id, null);

            if (EditedMessage is null)
            {
                return BadRequest();
            }

            if (!await TryUpdateModelAsync(EditedMessage, nameof(EditedMessage)))
            {
                return BadRequest();
            }

            ModelState.ClearValidationState(nameof(EditedMessage));
            if (!TryValidateModel(EditedMessage, nameof(EditedMessage)))
            {
                return BadRequest();
            }

            await _messageService.UpdateMessageAsync(EditedMessage);
            return new JsonResult("success");
        }

        public async Task<IActionResult> OnGetRemove(int id, int pageIndex)
        {
            var message = await _messageService.GetMessageAsync(id, Status.Normal);
            if (message is null)
            {
                return BadRequest();
            }

            message.Status = Status.RemovedByModerator;
            await _messageService.UpdateMessageAsync(message);

            return RedirectToPage("/Admin/Messages",
                new {pageIndex});
        }
    }
}