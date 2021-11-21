using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages
{
    public class Profile : MyPageModel
    {
        private readonly FileService _fileService;
        private readonly SessionService _sessionService;
        private readonly TripService _tripService;
        private readonly UserService _userService;

        public Profile(IWebHostEnvironment environment, UserService userService, FileService fileService,
                       SessionService sessionService, TripService tripService)
        {
            _userService = userService;
            _fileService = fileService;
            _sessionService = sessionService;
            _tripService = tripService;
        }

        public User CurrentUser { get; set; }

        [BindProperty]
        [Display(Name = "File")]
        public IFormFile Avatar { get; set; }

        public async Task<IActionResult> OnGet(int id, int pageNumber = 1)
        {
            const int pageSize = 10;

            CurrentUser = await _userService.GetUserAsync(id);
            if (CurrentUser is null)
            {
                return NotFound();
            }

            CurrentUser.JoinedTrips = await _tripService.GetJoinedTripsAsync(CurrentUser.Id, pageNumber, pageSize);

            return Page();
        }

        public async Task<JsonResult> OnPostEdit(int id)
        {
            CurrentUser = await _userService.GetUserAsync(id);
            if (CurrentUser?.Id != User?.Id)
            {
                return new JsonResult("Нет доступа!");
            }

            if (!await TryUpdateModelAsync(CurrentUser, "CurrentUser",
                u => u.Description,
                u => u.Email,
                u => u.Patronymic,
                u => u.BirthDate,
                u => u.FirstName,
                u => u.LastName) || !ModelState.IsValid)
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

        public async Task<IActionResult> OnGetLogOut()
        {
            if (HttpContext.Items["Session"] is not Session session)
            {
                return RedirectToPage("Index");
            }

            HttpContext.Response.Cookies.Append("token", session.Token, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(-1)
            });
            await _sessionService.RemoveSessionAsync(session);

            return RedirectToPage("Index");
        }
    }
}