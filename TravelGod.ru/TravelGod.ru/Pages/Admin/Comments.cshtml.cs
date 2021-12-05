using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Services;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Pages.Admin
{
    [AdministratorPageFilter]
    public class Comments : MyPageModel
    {
        private readonly CommentService _commentService;

        public Comments(CommentService commentService)
        {
            _commentService = commentService;
        }

        public PaginatedList<Comment> ListOfComments { get; private set; }
        public Comment EditedComment { get; set; }

        public async Task<IActionResult> OnGet(int pageIndex = 1)
        {
            ListOfComments = await _commentService.GetCommentsAsync(pageIndex);

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            EditedComment = await _commentService.GetCommentAsync(id, null);

            if (EditedComment is null)
            {
                return BadRequest();
            }

            if (!await TryUpdateModelAsync(EditedComment, nameof(EditedComment)))
            {
                return BadRequest();
            }

            ModelState.ClearValidationState(nameof(EditedComment));
            if (!TryValidateModel(EditedComment, nameof(EditedComment)))
            {
                return BadRequest();
            }

            await _commentService.UpdateCommentAsync(EditedComment);
            return new JsonResult("success");
        }

        public async Task<IActionResult> OnGetRemove(int id, int pageIndex)
        {
            var comment = await _commentService.GetCommentAsync(id, Status.Normal);
            if (comment is null)
            {
                return BadRequest();
            }

            comment.Status = Status.RemovedByModerator;
            await _commentService.UpdateCommentAsync(comment);

            return RedirectToPage("/Admin/Comments",
                new {pageIndex});
        }
    }
}