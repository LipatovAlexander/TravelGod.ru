using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services;
using File = TravelGod.ru.Models.File;
using Index = System.Index;

namespace TravelGod.ru.Pages
{
    public class Profile : MyPageModel
    {
        public User CurrentUser { get; set; }
        [BindProperty, Display(Name = "File")] public IFormFile Avatar { get; set; }

        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserService _userService;
        private readonly FileService _fileService;
        private readonly SessionService _sessionService;
        private readonly TripService _tripService;

        public Profile(IWebHostEnvironment environment, UserService userService, FileService fileService,
                       SessionService sessionService, TripService tripService)
        {
            _appEnvironment = environment;
            _userService = userService;
            _fileService = fileService;
            _sessionService = sessionService;
            _tripService = tripService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            CurrentUser = await _userService.GetUserAsync(id);
            if (CurrentUser is null)
            {
                return NotFound();
            }

            CurrentUser.Avatar = await _fileService.GetFileAsync(CurrentUser.AvatarId);
            CurrentUser.JoinedTrips = await _tripService.GetJoinedTrips(CurrentUser.Id);

            return Page();
        }

        public async Task<JsonResult> OnPostEdit(int id)
        {
            CurrentUser = await _userService.GetUserAsync(id);
            if (CurrentUser.Id != User.Id)
                return new JsonResult("Нет доступа!");

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

                var path = "CustomFiles/Avatars/" + CurrentUser.Id +
                           Path.GetExtension(Avatar.FileName).ToLowerInvariant();
                await using (var fileStream =
                    new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
                {
                    await Avatar.CopyToAsync(fileStream);
                }

                var file = new File {Name = Avatar.FileName, Path = path};
                await _fileService.AddFileAsync(file);
                CurrentUser.Avatar = file;
            }
            else
            {
                CurrentUser.Avatar = await _fileService.GetFileAsync(CurrentUser.AvatarId);
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

            HttpContext.Response.Cookies.Append("token", session.Token, new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddDays(-1)
            });
            await _sessionService.RemoveSessionAsync(session);

            return RedirectToPage("Index");
        }
    }
}