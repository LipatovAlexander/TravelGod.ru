using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages.Trips
{
    public class Concrete : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Concrete(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Trip Trip { get; set; }

        [FromForm] public Comment NewComment { get; set; }
        [BindProperty] public Rating NewRating { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Trip = await _unitOfWork.Trips.FirstOrDefaultAsync(
                t => t.Id == id && t.Status == Status.Normal,
                trips =>
                    trips.Include(t => t.Users.Where(u => u.Status == Status.Normal))
                         .ThenInclude(u => u.Avatar)
                         .Include(t => t.Chat)
                         .Include(t => t.Comments
                                        .Where(c => c.Status == Status.Normal)
                                        .OrderByDescending(c => c.CreatedAt))
                         .ThenInclude(c => c.CreatedBy.Avatar)
                         .Include(t => t.Ratings.Where(r => r.Status == Status.Normal)));
            if (Trip is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetJoin(int id)
        {
            if (User is null)
            {
                return BadRequest();
            }

            try
            {
                await _unitOfWork.Trips.AddUserAsync(id, User);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest();
            }

            return RedirectToPage("/Trips/Concrete", new {Id = id});
        }

        public async Task<IActionResult> OnGetCreateChat(int id)
        {
            if (User is null)
            {
                return BadRequest();
            }

            try
            {
                await _unitOfWork.Chats.CreateForTripAsync(id, User);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest();
            }

            return RedirectToPage("/Trips/Concrete", new {Id = id});
        }

        public async Task<IActionResult> OnPostAddComment(int id)
        {
            if (User is null || NewComment is null)
            {
                return BadRequest();
            }

            ModelState.Clear();
            if (!TryValidateModel(NewComment, nameof(NewComment)))
            {
                return Page();
            }

            try
            {
                await _unitOfWork.Comments.CreateForTripAsync(id, NewComment);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest();
            }

            return ViewComponent("Comment", new {Comment = NewComment});
        }

        public async Task<IActionResult> OnPostAddRating(int id)
        {
            if (User is null || NewRating is null)
            {
                return BadRequest();
            }

            ModelState.Clear();
            if (!TryValidateModel(NewRating))
            {
                return Page();
            }

            try
            {
                await _unitOfWork.Ratings.CreateForTripAsync(id, NewRating, User);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest();
            }

            return RedirectToPage("/Trips/Concrete", new {id});
        }
    }
}