using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages.Admin
{
    public class Ratings : MyPageModel
    {
        private readonly RatingService _ratingService;

        public Ratings(RatingService ratingService)
        {
            _ratingService = ratingService;
        }

        public PaginatedList<Rating> ListOfRatings { get; set; }
        public Rating EditedRating { get; set; }

        public async Task<IActionResult> OnGet(int pageIndex = 1)
        {
            ListOfRatings = await _ratingService.GetRatingsAsync(pageIndex);

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedRating = await _ratingService.GetRatingAsync(id, null);

            if (EditedRating is null)
            {
                return BadRequest();
            }

            if (!await TryUpdateModelAsync(EditedRating, nameof(EditedRating)))
            {
                return BadRequest();
            }

            ModelState.ClearValidationState(nameof(EditedRating));
            if (!TryValidateModel(EditedRating, nameof(EditedRating)))
            {
                return BadRequest();
            }

            await _ratingService.UpdateRatingAsync(EditedRating);
            return new JsonResult("success");
        }

        public async Task<IActionResult> OnGetRemove(int id, int pageIndex)
        {
            var message = await _ratingService.GetRatingAsync(id, Status.Normal);
            if (message is null)
            {
                return BadRequest();
            }

            message.Status = Status.RemovedByModerator;
            await _ratingService.UpdateRatingAsync(message);

            return RedirectToPage("/Admin/Ratings",
                new {pageIndex});
        }
    }
}