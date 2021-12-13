using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages.Admin
{
    public class Ratings : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Ratings(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PaginatedList<Rating> ListOfRatings { get; set; }
        public Rating EditedRating { get; set; }

        public async Task<IActionResult> OnGet(int pageIndex = 1)
        {
            const int pageSize = 10;
            ListOfRatings = await _unitOfWork.Ratings.GetPaginatedListAsync(pageSize, pageIndex, null,
                ratings => ratings
                           .Include(r => r.CreatedBy)
                           .Include(r => r.Trip));

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedRating = await _unitOfWork.Ratings.FindByIdAsync(id);

            ModelState.Clear();
            if (EditedRating is null
                || !await TryUpdateModelAsync(EditedRating, nameof(EditedRating))
                || !TryValidateModel(EditedRating, nameof(EditedRating)))
            {
                return BadRequest();
            }

            _unitOfWork.Ratings.Update(EditedRating);
            await _unitOfWork.SaveAsync();
            return new JsonResult("success");
        }

        public async Task<IActionResult> OnGetRemove(int id, int pageIndex)
        {
            try
            {
                await _unitOfWork.Ratings.RemoveAsync(id);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest();
            }

            return RedirectToPage("/Admin/Ratings",
                new {pageIndex});
        }
    }
}