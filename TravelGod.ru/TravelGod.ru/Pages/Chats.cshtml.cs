using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages
{
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
            Console.WriteLine(true);
            if (User is null)
            {
                return RedirectToPage("/SignIn");
            }

            ListOfChats = await _chatService.GetChatsAsync(User);
            return Page();
        }

        public async Task<IActionResult> OnPostSendMessage()
        {
            if (User is null)
            {
                return BadRequest();
            }

            Message.User = User;
            Message.Chat = await _chatService.GetChatAsync(Message.Chat.Id);
            await _messageService.AddMessageAsync(Message);
            return ViewComponent("Message", new {Message = Message});
        }

        public async Task<IActionResult> OnGetGetChat(int id)
        {
            var chat = await _chatService.GetChatAsync(id);
            return ViewComponent("MessagesBox", new {Chat = chat});
        }
    }
}