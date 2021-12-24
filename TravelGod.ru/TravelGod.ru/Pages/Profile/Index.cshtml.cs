using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
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

        public User CurrentUser { get; set; }

        [Display(Name = "File")]
        public IFormFile Avatar { get; set; }

        [BindProperty] public Message NewMessage { get; set; }

        public async Task<IActionResult> OnGet(int id, int pageNumber = 1)
        {
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
                if (!TryValidateModel(Avatar, nameof(Avatar)))
                {
                    return new JsonResult(new {Success = false});
                }
                CurrentUser.Avatar =
                    _unitOfWork.Avatars.CreateFromFormFile(Avatar, environment.WebRootPath,
                        CurrentUser.Id.ToString());
            }

            try
            {
                _unitOfWork.Users.Update(CurrentUser);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return new JsonResult(new {Success = false});
            }

            return new JsonResult(new {Success = true});
        }

        public async Task<IActionResult> OnGetLogOut(int id)
        {
            if (HttpContext.Items["Session"] is not Session session)
            {
                return RedirectToPage("/Index");
            }

            try
            {
                _unitOfWork.Sessions.Remove(session);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest();
            }

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

            try
            {
                await _unitOfWork.Chats.SendMessageAsync(User, CurrentUser, NewMessage);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest();
            }

            return RedirectToPage("/Profile/Chats");
        }
    }
}