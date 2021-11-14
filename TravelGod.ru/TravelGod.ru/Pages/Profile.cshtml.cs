using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages
{
    public class Profile : MyPageModel
    {
        public User CurrentUser { get; set; }

        public Profile(ApplicationContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGet(int id)
        {
            CurrentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (CurrentUser is null)
            {
                return NotFound();
            }

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

            _context.Users.Update(CurrentUser);
            await _context.SaveChangesAsync();
            return new JsonResult(new {Success = true});
        }
    }
}