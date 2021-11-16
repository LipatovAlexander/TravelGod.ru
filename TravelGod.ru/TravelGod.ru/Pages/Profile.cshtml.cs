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
using File = TravelGod.ru.Models.File;
using Index = System.Index;

namespace TravelGod.ru.Pages
{
    public class Profile : MyPageModel
    {
        public User CurrentUser { get; set; }
        [BindProperty, Display(Name="File")]
        public IFormFile Avatar { get; set; }

        private readonly IWebHostEnvironment _appEnvironment;

        public Profile(ApplicationContext context, IWebHostEnvironment environment) : base(context)
        {
            _appEnvironment = environment;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            CurrentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (CurrentUser is null)
            {
                return NotFound();
            }
            await _context.Entry(CurrentUser).Collection(u => u.JoinedTrips).LoadAsync();
            foreach (var trip in CurrentUser.JoinedTrips)
            {
                await _context.Entry(trip).Collection(t => t.Users).LoadAsync();
            }

            await _context.Entry(CurrentUser).Reference(u => u.Avatar).LoadAsync();

            return Page();
        }

        public async Task<JsonResult> OnPostEdit(int id)
        {
            CurrentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
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

                // путь к папке Files
                string path = "CustomFiles/Avatars/" + CurrentUser.Id + Path.GetExtension(Avatar.FileName).ToLowerInvariant();
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
                {
                    await Avatar.CopyToAsync(fileStream);
                }
                var file = new File { Name = Avatar.FileName, Path = path };
                _context.Files.Add(file);
                await _context.SaveChangesAsync();
                CurrentUser.Avatar = file;
            }

            _context.Users.Update(CurrentUser);
            await _context.SaveChangesAsync();
            return new JsonResult(new {Success = true});
        }

        public async Task<IActionResult> OnGetLogOut()
        {
            var session = HttpContext.Items["Session"] as Session;
            if (session is not null)
            {
                HttpContext.Response.Cookies.Append("token", session.Token, new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddDays(-1)
                });
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}