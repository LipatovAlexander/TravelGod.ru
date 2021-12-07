using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages.Profile
{
    public class Index : MyPageModel
    {
        private readonly FileService _fileService;
        private readonly SessionService _sessionService;
        private readonly TripService _tripService;
        private readonly UserService _userService;
        private readonly ChatService _chatService;
        private readonly MessageService _messageService;

        public Index(IWebHostEnvironment environment, UserService userService, FileService fileService,
                     SessionService sessionService, TripService tripService, ChatService chatService,
                     MessageService messageService)
        {
            _userService = userService;
            _fileService = fileService;
            _sessionService = sessionService;
            _tripService = tripService;
            _chatService = chatService;
            _messageService = messageService;
        }

        [BindProperty]
        public User CurrentUser { get; set; }

        [Display(Name = "File")]
        public IFormFile Avatar { get; set; }

        [BindProperty] public Message NewMessage { get; set; }

        public async Task<IActionResult> OnGet(int id, int pageNumber = 1)
        {
            const int pageSize = 10;

            CurrentUser = User?.Id == id
                ? User
                : await _userService.GetUserAsync(id, Status.Normal);
            if (CurrentUser is null)
            {
                return NotFound();
            }

            CurrentUser.JoinedTrips = await _tripService.GetJoinedTripsAsync(CurrentUser.Id, pageNumber, pageSize);

            return Page();
        }

        public async Task<JsonResult> OnPostEdit(int id)
        {
            CurrentUser = await _userService.GetUserAsync(id, Status.Normal);
            if (CurrentUser?.Id != User?.Id)
            {
                return new JsonResult("Нет доступа!");
            }

            ModelState.Clear();
            if (!await TryUpdateModelAsync(CurrentUser, "CurrentUser",
                u => u.Description,
                u => u.Email,
                u => u.Patronymic,
                u => u.BirthDate,
                u => u.FirstName,
                u => u.LastName) || !TryValidateModel(CurrentUser, nameof(CurrentUser)))
            {
                return new JsonResult(new {Success = false});
            }

            if (Avatar != null)
            {
                if (!ImageValidator.IsValid(Avatar))
                {
                    return new JsonResult(new {Success = false});
                }

                CurrentUser.Avatar = await _fileService.AddFileAsync(CurrentUser, Avatar);
            }

            await _userService.UpdateUserAsync(CurrentUser);
            return new JsonResult(new {Success = true});
        }

        public async Task<IActionResult> OnGetLogOut(int id)
        {
            if (HttpContext.Items["Session"] is not Session session)
            {
                return RedirectToPage("/Index");
            }

            HttpContext.Response.Cookies.Append("token", session.Token, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(-1)
            });
            await _sessionService.RemoveSessionAsync(session);

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostSendMessage(int id)
        {
            if (User is null)
            {
                return BadRequest();
            }

            CurrentUser = await _userService.GetUserAsync(id, Status.Normal);
            if (CurrentUser is null || CurrentUser.Id == User.Id)
            {
                return BadRequest();
            }

            ModelState.Clear();
            if (!TryValidateModel(NewMessage, nameof(NewMessage)))
            {
                return Page();
            }

            var chat = await _chatService.GetChatAsync(false, User, CurrentUser)
                       ?? new Chat
                       {
                           Initiator = User,
                           IsGroupChat = false,
                           Users = new List<User> {User, CurrentUser}
                       };

            NewMessage.Chat = chat;
            NewMessage.User = User;
            NewMessage.DateTime = DateTime.Now;
            await _messageService.AddMessageAsync(NewMessage);
            return RedirectToPage("/Profile/Chats");
        }
    }
}