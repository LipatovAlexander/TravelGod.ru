using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using TravelGod.ru.Areas.Admin.ViewModels;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Pages;
using TravelGod.ru.Services;

namespace TravelGod.ru.Areas.Admin.Pages
{
    public class Users : MyPageModel
    {
        private readonly UserService _userService;

        public Users(UserService userService)
        {
            _userService = userService;
        }

        public PaginatedList<User> ListOfUsers { get; private set; }
        [BindProperty(SupportsGet = true)] public UsersOptions UsersOptions { get; set; }
        public User EditedUser { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (User is null || User.Role is not (Role.Administrator or Role.Moderator))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            ListOfUsers = await _userService.GetUsersAsync(UsersOptions);

            return Page();
        }

        public async Task<IActionResult> OnPostEditUser(int id)
        {
            if (User is null || User.Role is not (Role.Administrator or Role.Moderator))
            {
                return BadRequest();
            }

            EditedUser = await _userService.GetUserAsync(id);

            if (EditedUser is null)
            {
                return BadRequest();
            }

            if (!await TryUpdateModelAsync(EditedUser, "EditedUser"))
            {
                return BadRequest();
            }

            ModelState.ClearValidationState(nameof(EditedUser));
            if (!TryValidateModel(EditedUser, "EditedUser"))
            {
                return BadRequest();
            }

            await _userService.UpdateUserAsync(EditedUser);
            return new JsonResult("success");
        }
    }
}