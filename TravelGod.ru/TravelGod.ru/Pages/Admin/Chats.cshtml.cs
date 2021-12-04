using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages.Admin
{
    public class Chats : MyPageModel
    {
        private readonly ChatService _chatService;

        public Chats(ChatService chatService)
        {
            _chatService = chatService;
        }

        public PaginatedList<Chat> ListOfChats { get; set; }
        public Chat EditedChat { get; set; }

        public async Task<IActionResult> OnGet(int pageIndex = 1)
        {
            ListOfChats = await _chatService.GetChatsAsync(pageIndex);

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedChat = await _chatService.GetChatAsync(id, null);

            if (EditedChat is null)
            {
                return BadRequest();
            }

            if (!await TryUpdateModelAsync(EditedChat, nameof(EditedChat)))
            {
                return BadRequest();
            }

            ModelState.ClearValidationState(nameof(EditedChat));
            if (!TryValidateModel(EditedChat, nameof(EditedChat)))
            {
                return BadRequest();
            }

            await _chatService.UpdateChatAsync(EditedChat);
            return new JsonResult("success");
        }

        public async Task<IActionResult> OnGetRemove(int id, int pageIndex)
        {
            var chat = await _chatService.GetChatAsync(id, Status.Normal);
            if (chat is null)
            {
                return BadRequest();
            }

            await _chatService.RemoveChatAsync(chat);

            return RedirectToPage("/Admin/Chats",
                new {pageIndex});
        }
    }
}