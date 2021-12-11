using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages.Admin
{
    [AdministratorPageFilter]
    public class Comments : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Comments(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PaginatedList<Comment> ListOfComments { get; private set; }
        public Comment EditedComment { get; set; }

        public async Task<IActionResult> OnGet(int pageIndex = 1)
        {
            const int pageSize = 10;
            ListOfComments = await _unitOfWork.Comments.GetPaginatedListAsync(pageSize, pageIndex, null,
                comments => comments
                            .Include(c => c.Trip)
                            .Include(c => c.CreatedBy.Avatar));

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedComment = await _unitOfWork.Comments.FindByIdAsync(id);

            ModelState.Clear();
            if (EditedComment is null
                || !await TryUpdateModelAsync(EditedComment, nameof(EditedComment))
                || !TryValidateModel(EditedComment, nameof(EditedComment)))
            {
                return BadRequest();
            }

            _unitOfWork.Comments.Update(EditedComment);
            await _unitOfWork.SaveAsync();
            return new JsonResult("success");
        }

        public async Task<IActionResult> OnGetRemove(int id, int pageIndex)
        {
            var comment = await _unitOfWork.Comments.FindByIdAsync(id);
            if (comment?.Status is not Status.Normal)
            {
                return BadRequest();
            }

            comment.Status = Status.RemovedByModerator;
            _unitOfWork.Comments.Update(comment);
            await _unitOfWork.SaveAsync();

            return RedirectToPage("/Admin/Comments",
                new {pageIndex});
        }
    }
}