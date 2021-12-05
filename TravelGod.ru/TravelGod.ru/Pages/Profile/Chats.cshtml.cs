using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages.Profile
{
    [AuthenticationPageFilter]
    public class Chats : MyPageModel
    {
        private readonly ChatService _chatService;
        private readonly MessageService _messageService;

        public Chats(ChatService chatService, MessageService messageService)
        {
            _chatService = chatService;
            _messageService = messageService;
        }

        public List<Chat> ListOfChats { get; set; }

        [FromForm]
        public Message Message { get; set; }

        public async Task<IActionResult> OnGet()
        {
            ListOfChats = await _chatService.GetChatsAsync(User, Status.Normal);
            return Page();
        }

        public async Task<IActionResult> OnPostSendMessage()
        {
            Message.User = User;
            Message.Chat = await _chatService.GetChatAsync(Message.Chat.Id, Status.Normal);
            await _messageService.AddMessageAsync(Message);
            return ViewComponent("Message", new {Message = Message});
        }

        public async Task<IActionResult> OnGetGetChat(int id)
        {
            var chat = await _chatService.GetChatAsync(id, Status.Normal);
            if (chat.Status == Status.Normal)
            {
                return ViewComponent("MessagesBox", new {Chat = chat});
            }
            else
            {
                return BadRequest();
            }
        }
    }
}