using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services.Filters;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Pages.Admin
{
    [AdministratorPageFilter]
    public class Users : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Users(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PaginatedList<User> ListOfUsers { get; private set; }
        [BindProperty(SupportsGet = true)] public UserFilter UserFilter { get; set; }
        public User EditedUser { get; set; }

        public async Task<IActionResult> OnGet(string name, Role role, Status status)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            const int pageSize = 10;
            ListOfUsers = await _unitOfWork.Users.GetPaginatedListAsync(pageSize, UserFilter.PageNumber, UserFilter);

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedUser = await _unitOfWork.Users.FindByIdAsync(id);

            ModelState.Clear();
            if (EditedUser is null
                || !await TryUpdateModelAsync(EditedUser, nameof(EditedUser))
                || !TryValidateModel(EditedUser, nameof(EditedUser)))
            {
                return BadRequest();
            }

            _unitOfWork.Users.Update(EditedUser);
            await _unitOfWork.SaveAsync();
            return new JsonResult("success");
        }

        public async Task<IActionResult> OnPostRemove(int id)
        {
            var user = await _unitOfWork.Users.FindByIdAsync(id);
            if (user?.Status is not Status.Normal || user.Id == User.Id)
            {
                return BadRequest();
            }

            user.Status = Status.RemovedByModerator;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAsync();

            return new OkResult();
        }
    }
}