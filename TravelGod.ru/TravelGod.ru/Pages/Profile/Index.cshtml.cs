using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages.Profile
{
    public class Index : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Index(IWebHostEnvironment environment, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty] public User CurrentUser { get; set; }

        [Display(Name = "File")] public IFormFile Avatar { get; set; }

        [BindProperty] public Message NewMessage { get; set; }

        public async Task<IActionResult> OnGet(int id, int pageNumber = 1)
        {
            const int pageSize = 10;

            CurrentUser = await _unitOfWork.Users.FirstOrDefaultAsync(
                u => u.Id == id && u.Status == Status.Normal,
                source => source
                          .Include(u => u.JoinedTrips
                                         .OrderByDescending(t => t.EndDate)
                                         .Where(t => t.Status == Status.Normal))
                          .Include(u => u.Avatar));
            if (CurrentUser is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<JsonResult> OnPostEdit(int id, [FromServices] IWebHostEnvironment environment)
        {
            CurrentUser = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status == Status.Normal,
                source => source.Include(u => u.Avatar));
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

                CurrentUser.Avatar =
                    await _unitOfWork.Avatars.CreateFromFormFileAsync(Avatar, environment.WebRootPath,
                        CurrentUser.Id.ToString());
            }

            _unitOfWork.Users.Update(CurrentUser);
            await _unitOfWork.SaveAsync();
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
            _unitOfWork.Sessions.Remove(session);
            await _unitOfWork.SaveAsync();

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostSendMessage(int id)
        {
            if (User is null)
            {
                return BadRequest();
            }

            CurrentUser = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status == Status.Normal,
                source => source
                          .Include(u => u.JoinedTrips.Where(t => t.Status == Status.Normal))
                          .Include(u => u.JoinedChats.Where(c => c.Status == Status.Normal))
                          .ThenInclude(c => c.Users.Where(u => u.Status == Status.Normal))
                          .Include(u => u.Avatar));
            if (CurrentUser is null || CurrentUser.Id == User.Id)
            {
                return BadRequest();
            }

            ModelState.Clear();
            if (!TryValidateModel(NewMessage, nameof(NewMessage)))
            {
                return Page();
            }

            var chat = CurrentUser.JoinedChats.FirstOrDefault(c =>
                           !c.IsGroupChat && c.Users.Select(u => u.Id).Contains(User.Id))
                       ?? new Chat
                       {
                           Users = new List<User> {User, CurrentUser}
                       };

            NewMessage.Chat = chat;
            _unitOfWork.Messages.Create(NewMessage);
            await _unitOfWork.SaveAsync();
            return RedirectToPage("/Profile/Chats");
        }
    }
}